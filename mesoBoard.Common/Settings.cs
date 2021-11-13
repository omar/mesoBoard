using System.Collections.Specialized;
using System.Configuration;
using System.Xml.Linq;
using System.Linq;
using System.Net.Mail;
using System;
using System.IO;

namespace mesoBoard
{
    public static class Settings
    {
        public static readonly string ConnectionStringName = "mesoBoard";
        public static readonly string EntityConnectionStringName = "mesoBoardEntity";

        public static readonly string ConnectionStringTemplate = @"
            Data Source={dbserver};
            Initial Catalog={dbname};
            Integrated Security={integratedsecurity};
            User ID={dblogin};Password={dbpassword};
            MultipleActiveResultSets=True";

        public static readonly string EntityConnectionStringTemplate = @"
            metadata=res://*/Entities.SqlServer.csdl|res://*/Entities.SqlServer.ssdl|res://*/Entities.SqlServer.msl;
            provider=System.Data.SqlClient;
            provider connection string='{CONNECTIONSTRING}'";

        /// <summary>
        /// Connection string used to connect to the mesoBoard installation database. 
        /// If a connection string named 'mesoBoard' is found in Web.config, that value is returned. 
        /// Otherwise, a connection string is generated based on the database values in Settings.config
        /// </summary>
        public static string ConnectionString { get { return GenerateConnectionString(); } }

        /// <summary>
        /// Entity connection string used by Entity Framework data contexts to connect to the mesoBoard installation database
        /// If a connection string named 'mesoBoardEntity' is found in Web.config, that value is returned. 
        /// Otherwise, a connection string is generated based on the database values in Settings.config
        /// </summary>
        public static string EntityConnectionString { get { return GenerateEntityConnectionString(); } }

        public static bool IsInstalled 
        { 
            get { return bool.Parse(GetAppSetting(SettingKeys.Installed)); }
            set { SetAppSetting(SettingKeys.Installed, value.ToString()); }
        }

        public static string DatabaseServer 
        { 
            get { return GetAppSetting(SettingKeys.DatabaseServer); }
            set { SetAppSetting(SettingKeys.DatabaseServer, value); }
        }

        public static bool UseIntegratedSecurity
        {
            get { return bool.Parse(GetAppSetting(SettingKeys.UseIntegratedSecurity)); }
            set { SetAppSetting(SettingKeys.UseIntegratedSecurity, value.ToString()); }
        }

        public static string DatabaseName
        {
            get { return GetAppSetting(SettingKeys.DatabaseName); }
            set { SetAppSetting(SettingKeys.DatabaseName, value); }
        }

        public static string DatabaseLogin
        {
            get { return GetAppSetting(SettingKeys.DatabaseLogin); }
            set { SetAppSetting(SettingKeys.DatabaseLogin, value); }
        }

        public static string DatabasePassword
        {
            get { return GetAppSetting(SettingKeys.DatabasePassword); }
            set { SetAppSetting(SettingKeys.DatabasePassword, value); }
        }

        public static string SmtpServer
        {
            get { return GetAppSetting(SettingKeys.SmtpServer); }
            set { SetAppSetting(SettingKeys.SmtpServer, value); }
        }

        public static int SmtpPort
        {
            get { return int.Parse(GetAppSetting(SettingKeys.SmtpPort)); }
            set { SetAppSetting(SettingKeys.SmtpPort, value.ToString()); }
        }

        public static bool SmtpUseDefaultCredentials
        {
            get { return bool.Parse(GetAppSetting(SettingKeys.SmtpUseDefaultCredentials)); }
            set { SetAppSetting(SettingKeys.SmtpUseDefaultCredentials, value.ToString()); }
        }

        public static string SmtpLogin
        {
            get { return GetAppSetting(SettingKeys.SmtpLogin); }
            set { SetAppSetting(SettingKeys.SmtpLogin, value); }
        }

        public static string SmtpPassword
        {
            get { return GetAppSetting(SettingKeys.SmtpPassword); }
            set { SetAppSetting(SettingKeys.SmtpPassword, value); }
        }

        /// <summary>
        /// Returns a configured SmtpClient. Use settings from 'mailSettings/smtp' if they are set.
        /// Otherwise, generates connection settings based on the Smtp values in Settings.config
        /// </summary>
        public static SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            if (client.Host == null && client.Credentials == null && client.DeliveryMethod == SmtpDeliveryMethod.Network)
            {
                client.Host = SmtpServer;
                client.Port = SmtpPort;
                client.UseDefaultCredentials = SmtpUseDefaultCredentials;
                if(!SmtpUseDefaultCredentials)
                    client.Credentials = new System.Net.NetworkCredential(SmtpLogin, SmtpPassword);
            }
            return client;
        }

        /// <summary>
        /// Generates a valid SQL Server connection string based on the database settings provided in the Settings.config file
        /// </summary>
        private static string GenerateConnectionString()
        {
            var webConfigConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName];
            if (webConfigConnectionString != null)
                return webConfigConnectionString.ConnectionString;


            /* Place holders: */
            // {dbserver}
            // {integratedsecurity}
            // {dbname}
            // {dblogin}
            // {dbpassword}

            string connectionString =
                        ConnectionStringTemplate
                            .Replace("{dbserver}", DatabaseServer)
                            .Replace("{integratedsecurity}", UseIntegratedSecurity.ToString()) 
                            .Replace("{dbname}", DatabaseName)
                            .Replace("{dblogin}", DatabaseLogin)
                            .Replace("{dbpassword}", DatabasePassword);

            return connectionString;
        }

        private static string GenerateEntityConnectionString()
        {
            var webConfigConnectionString = ConfigurationManager.ConnectionStrings[EntityConnectionStringName];
            if (webConfigConnectionString != null)
                return webConfigConnectionString.ConnectionString;

            /* Place holders: */
            // {CONNECTIONSTRING}

            string connectionString = EntityConnectionStringTemplate.Replace("{CONNECTIONSTRING}", ConnectionString);

            return connectionString;
        }

        private static void SetAppSetting(string settingKey, string value)
        {
            var settingsFilePath = "appsettings.config";
            string json = File.ReadAllText(settingsFilePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj[settingKey] = value;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(settingsFilePath, output);
        }

        private static string GetAppSetting(string settingKey)
        {
            var settingsFilePath = "appsettings.config";
            string json = File.ReadAllText(settingsFilePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return jsonObj[settingKey];
        }
    }
}
