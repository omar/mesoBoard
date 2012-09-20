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

        public UpgradeServices(
            mbEntities dataContext)
        {
            _dataContext = dataContext;
        }

        public string[] GetAvailableUpgrades()
        {
            return new string[] { };
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
