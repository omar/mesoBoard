using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Models;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace mesoBoard.Web.Controllers
{
    public class InstallController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IRepository<User> _userRepository;
        private UserServices _userServices;
        private SiteConfig _siteConfig;
        private static string SessionSqlInfoKey = "mbSqlInfoKey";
        private static string SessionMailInfoKey = "mbMailInfoKey";

        public InstallController(
            IRepository<User> userRepository,
            UserServices userServices,
            IUnitOfWork unitOfWork,
            SiteConfig siteConfig)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userServices = userServices;
            _siteConfig = siteConfig;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items[HttpContextItemKeys.ThemeFolder] = "Default";
            
            if (Settings.IsInstalled)
                View("AlreadyInstalled").ExecuteResult(ControllerContext);
            else
                base.OnActionExecuting(context);
        }

        public ActionResult Index()
        {
            ViewData["BreadCurmb"] = "Welcome";

            return View();
        }

        [HttpGet]
        public ActionResult Step1()
        {
            ViewData["StepNumber"] = 1;
            SQLInstallViewModel model = new SQLInstallViewModel
            {
                DatabaseServer = "localhost",
                UseIntegratedSecurity = false,
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Step1(SQLInstallViewModel info)
        {
            ViewData["StepNumber"] = 1;

            if (info.UseIntegratedSecurity)
            {
                ModelState.Remove("DatabaseLogin");
                ModelState.Remove("DatabasePassword");
            }

            if (!ModelState.IsValid)
                return View(info);

            var connectionString = new SqlConnectionStringBuilder();

            connectionString.DataSource = info.DatabaseServer;
            connectionString.InitialCatalog = info.DatabaseName;
            connectionString.MultipleActiveResultSets = true;
            connectionString.IntegratedSecurity = info.UseIntegratedSecurity; ;

            if (!info.UseIntegratedSecurity)
            {
                connectionString.UserID = info.DatabaseLogin;
                connectionString.Password = info.DatabasePassword;
            }

            var connection = new SqlConnection(connectionString.ToString());

            try
            {
                connection.Open();
                string sql = System.IO.File.ReadAllText("~/App_Data/Install/mesoBoard.SqlServer.sql");

                string[] cmds = sql.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                foreach (string cmd in cmds)
                {
                    command.CommandText = cmd;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                TempData[ViewDataKeys.GlobalMessages.Notice] = "Unable to connect to SQL server, check connection information";
                TempData[ViewDataKeys.GlobalMessages.Error] = ex.Message;
                return View(info);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            HttpContext.Session.SetObject(SessionSqlInfoKey, info);

            return RedirectToAction("Step2");
        }

        [HttpGet]
        public ActionResult Step2()
        {
            ViewData["StepNumber"] = 2;

            if (HttpContext.Session.GetObject<SQLInstallViewModel>(SessionSqlInfoKey) != null)
                ViewData[ViewDataKeys.GlobalMessages.Success] = "Successfully connected to database and created tables";

            MailInstallViewModel model = new MailInstallViewModel
            {
                MailServerAddress = "mail." + Request.Host,
                MailUseDefaultCredentials = false,
                PortNumber = 25
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Step2(MailInstallViewModel info)
        {
            ViewData["StepNumber"] = 2;

            if (info.MailUseDefaultCredentials)
            {
                ModelState.Remove("MailLogin");
                ModelState.Remove("MailPassword");
            }

            if (!ModelState.IsValid)
                return View(info);

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = info.MailServerAddress;
            smtpClient.Port = info.PortNumber;
            smtpClient.UseDefaultCredentials = info.MailUseDefaultCredentials;
            if (!info.MailUseDefaultCredentials)
                smtpClient.Credentials = new NetworkCredential(info.MailLogin, info.MailPassword);

            try
            {
                smtpClient.Send("no-reply@" + Request.Host, "test@" + smtpClient.Host, "", "");
            }
            catch (Exception ex)
            {
                ViewData[ViewDataKeys.GlobalMessages.Notice] = "Error connecting to SMTP server, a " + ex.GetType().ToString() + " exception was raised";
                ViewData[ViewDataKeys.GlobalMessages.Error] = ex.Message;
                return View(info);
            }

            HttpContext.Session.SetObject(SessionMailInfoKey, info);
            return RedirectToAction("Step3");
        }

        [HttpGet]
        public ActionResult Step3()
        {
            if (HttpContext.Session.GetObject<MailInstallViewModel>(SessionMailInfoKey) != null)
                ViewData[ViewDataKeys.GlobalMessages.Success] = "Successfully connected to mail server";

            var sqlInfo = HttpContext.Session.GetObject<SQLInstallViewModel>(SessionSqlInfoKey);
            if (sqlInfo != null)
            {
                Settings.DatabaseServer = sqlInfo.DatabaseServer;
                Settings.DatabaseName = sqlInfo.DatabaseName;
                Settings.UseIntegratedSecurity = sqlInfo.UseIntegratedSecurity;
                if (!sqlInfo.UseIntegratedSecurity)
                {
                    Settings.DatabaseLogin = sqlInfo.DatabaseLogin;
                    Settings.DatabasePassword = sqlInfo.DatabasePassword;
                }

                string connectionString =
                    Settings.ConnectionStringTemplate
                            .Replace("{dbserver}", sqlInfo.DatabaseServer)
                            .Replace("{integratedsecurity}", sqlInfo.UseIntegratedSecurity.ToString())
                            .Replace("{dbname}", sqlInfo.DatabaseName)
                            .Replace("{dblogin}", sqlInfo.DatabaseLogin)
                            .Replace("{dbpassword}", sqlInfo.DatabasePassword);

                string entityConnectionString = Settings.EntityConnectionStringTemplate.Replace("{CONNECTIONSTRING}", connectionString);
                mbEntities dataContext = new mbEntities(entityConnectionString);

                Config automatedEmail = dataContext.Configs.First(item => item.Name.Equals("AutomatedFromEmail"));
                automatedEmail.Value = "no-reply@" + Request.Host;

                Config boardUrl = dataContext.Configs.First(item => item.Name.Equals("BoardURL"));
                boardUrl.Value = Request.Host.ToString();

                dataContext.SaveChanges();
            }

            MailInstallViewModel mailInfo = HttpContext.Session.GetObject<MailInstallViewModel>(SessionMailInfoKey);
            if (mailInfo != null)
            {
                Settings.SmtpServer = mailInfo.MailServerAddress;
                Settings.SmtpPort = mailInfo.PortNumber;
                Settings.SmtpUseDefaultCredentials = mailInfo.MailUseDefaultCredentials;
                if (!mailInfo.MailUseDefaultCredentials)
                {
                    Settings.SmtpLogin = mailInfo.MailLogin;
                    Settings.SmtpPassword = mailInfo.MailPassword;
                }
            }

            ViewData["StepNumber"] = 3;
            return View();
        }

        [HttpPost]
        public ActionResult Step3(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                ModelState.AddModelError("username", "Enter an admin username");

            if (string.IsNullOrWhiteSpace(username) || !RegEx.IsValidEmail(email))
                ModelState.AddModelError("email", "Enter a valid email address");

            if (!ModelState.IsValid)
                return View();

            string password = Randoms.RandomPassword();
            string salt = Randoms.CreateSalt();
            string hashedPassword = _userServices.HashPassword(password, salt);

            string connectionString = Settings.EntityConnectionString;

            var date = DateTime.UtcNow;
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            User adminUser = _userRepository.Get().FirstOrDefault();

            adminUser.Email = email;
            adminUser.LastLoginIP = ipAddress;
            adminUser.Password = hashedPassword;
            adminUser.PasswordSalt = salt;
            adminUser.Username = username;
            adminUser.UsernameLower = username.ToLower();
            adminUser.RegisterIP = ipAddress;

            _userRepository.Update(adminUser);
            _unitOfWork.Commit();

            // If the Smtp server is 'mail.yourdomain.com', that means the user didn't specify mail settings
            // should probably store a value in the session indicating if they specified mail settings
            if (!Settings.SmtpServer.Equals("mail.yourdomain.com"))
            {
                using (SmtpClient smtp = Settings.GetSmtpClient())
                {
                    try
                    {
                        smtp.Send("no-reply@" + Request.Host,
                            email,
                            "mesoBoard Installation Complete",
                            "Admin user created" + Environment.NewLine +
                            "Username: " + username +
                            Environment.NewLine +
                            "Password: " + password);
                    }
                    catch
                    {
                        ViewData[ViewDataKeys.GlobalMessages.Error] = "Couldn't send email. Please store the login information below for your records";
                    }
                }
            }
            else
                ViewData[ViewDataKeys.GlobalMessages.Notice] = "Mail server settings were not given. No email was sent with login information. Please store the login information below for your records";

            ViewData["Username"] = username;
            ViewData["Password"] = password;
            ViewData["Email"] = email;
            ViewData[ViewDataKeys.GlobalMessages.Success] = "mesoBoard installation complete";

            try
            {
                _siteConfig.UpdateCache();
            }
            catch
            {
                ViewData[ViewDataKeys.GlobalMessages.Notice] = "Configuration Cache did not update. Please update manually through the Admin Control Panel";
            }

            Settings.IsInstalled = true;
            return View("InstallationComplete");
        }
    }
}