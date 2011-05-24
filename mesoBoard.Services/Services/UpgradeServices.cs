using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mesoBoard.Data;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Hosting;
using System.Data.SqlClient;
using mesoBoard.Common;
using System.Configuration;
using System.Net.Configuration;

namespace mesoBoard.Services
{
    public class UpgradeServices
    {
        private class UpgradeScripts
        {
            public UpgradeScripts(IEnumerable<UpgradeScript> scripts)
            {
                Scripts = scripts.Where(item => item.PreScript == false).OrderBy(item => item.Order).ToList();
                PreScripts = scripts.Where(item => item.PreScript == true).OrderBy(item => item.Order).ToList();
            }

            public List<UpgradeScript> Scripts { get; set; }
            public List<UpgradeScript> PreScripts { get; set; }
        }

        private class UpgradeScript
        {
            public UpgradeScript(string path)
            {
                FullPath = path;
                FileName = Path.GetFileName(path);

                var split = FileName.Split('-');
                var firstSplit = split[0];
                int result = 0;

                if (firstSplit == "pre")
                {
                    int.TryParse(split[1], out result);
                    PreScript = true;
                }
                else
                {
                    int.TryParse(firstSplit, out result);
                    PreScript = false;
                }
                
                
                Order = result;
            }

            public bool PreScript { get; set; }
            public int Order { get; set; }
            public string FileName { get; set; }
            public string FullPath { get; set; }
        }

        private mbEntities _dataContext;

        public UpgradeServices(mbEntities dataContext)
        {
            _dataContext = dataContext;
        }

        public string[] GetAvailableUpgrades()
        {
            return new string[] { "0.9.2", "0.9.3" };
        }

        private UpgradeScripts GetUpgradeScripts(string version)
        {
            string path = HostingEnvironment.MapPath(DirectoryPaths.Upgrade);
            path = Path.Combine(path, version);
            var sqlFiles = Directory.GetFiles(path, "*.sql");

            var upgradeScripts = sqlFiles.Select(item => new UpgradeScript(item));
            var scripts = new UpgradeScripts(upgradeScripts);
            return scripts;
        }

        public void To093()
        {
            // Create an SmtpClient to get the default values from the mailSettings section
            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            Settings.SmtpServer = smtpClient.Host ?? string.Empty;
            Settings.SmtpPort = smtpClient.Port;
            Settings.SmtpUseDefaultCredentials = smtpClient.UseDefaultCredentials;

            // Need to make sure all smtp settings are added to 'Settings.config'.
            // When setting a value for a setting and the setting doesn't exist in 'Settings.config'
            // the key is automatically added.
            Settings.SmtpLogin = string.Empty;
            Settings.SmtpPassword = string.Empty;

            if (!smtpClient.UseDefaultCredentials)
            {
                System.Net.NetworkCredential credentials = (System.Net.NetworkCredential)smtpClient.Credentials;
                if (credentials != null)
                {
                    Settings.SmtpLogin = credentials.UserName ?? string.Empty;
                    Settings.SmtpPassword = credentials.Password ?? string.Empty;
                }
            }

            var scripts = GetUpgradeScripts("0.9.3");

            foreach (var script in scripts.PreScripts)
                ExecuteUpgradeScript(script);

            Role defaultRole = _dataContext.Roles.FirstOrDefault(item => item.Name == "Member");
            if (defaultRole != null)
            {
                Config registrationRole = _dataContext.Configs.First(item => item.Name == "RegistrationRole");
                registrationRole.Value = defaultRole.RoleID.ToString();
            }

            foreach (ForumPermission forumPermission in _dataContext.ForumPermissions.Where(item => item.Role.Name == "Guest"))
            {
                forumPermission.Forum.VisibleToGuests = (forumPermission.Visibility == VisibilityPermissions.Visible.Value);
                forumPermission.Forum.AllowGuestDownloads = (forumPermission.Attachments == AttachmentPermissions.Download.Value);
            }

            _dataContext.SaveChanges();

            foreach (var script in scripts.Scripts)
                ExecuteUpgradeScript(script);

            SiteConfig.UpdateCache();
        }

        public void To092()
        {
            var scripts = GetUpgradeScripts("0.9.2");

            foreach (var script in scripts.PreScripts)
                ExecuteUpgradeScript(script);

            var posts = _dataContext.Posts;
            foreach (var post in posts)
            {
                post.TextOnly = Regex.Replace(post.Text, @"\[(.|\n)*?\]", string.Empty);
            }
            _dataContext.SaveChanges();

            foreach (var subscription in _dataContext.Subscriptions)
            {
                // in the previous version there was some issues with 
                // invalid subscriptions being created

                var thread = _dataContext.Threads.FirstOrDefault(item => item.ThreadID == subscription.ThreadID);
                var user = _dataContext.Users.FirstOrDefault(item => item.UserID == subscription.UserID);
                if (user == null || thread == null)
                {
                    _dataContext.DeleteObject(subscription);
                }
            }

            _dataContext.SaveChanges();

            foreach (var script in scripts.Scripts)
                ExecuteUpgradeScript(script);

            string uploadPath = HostingEnvironment.MapPath("~/Upload/Files");
            var attachments = Directory.GetFiles(uploadPath);

            foreach (var file in attachments)
            {
                string filename = Path.GetFileName(file);
                string newpath = Path.Combine(DirectoryPaths.Attachments, filename);
                File.Move(file, newpath);
            }

            Directory.Delete(uploadPath, true);

        }

        private void ExecuteUpgradeScript(UpgradeScript upgradeScript)
        {
            var connection = new SqlConnection(Settings.ConnectionString);

            string sql = System.IO.File.ReadAllText(upgradeScript.FullPath);

            string[] cmds = sql.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            connection.Open();
            foreach (string cmd in cmds)
            {
                command.CommandText = cmd;
                command.ExecuteNonQuery();
            }
            connection.Close();
            connection.Dispose();

        }
    }
}
