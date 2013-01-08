using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Services;
using mesoBoard.Web.Areas.Admin.Models;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public class PluginsController : BaseAdminController
    {
        private IRepository<PluginConfig> _pluginConfigRepository;
        private IRepository<Plugin> _pluginRepository;
        private PluginServices _pluginServices;

        public PluginsController(
            IRepository<PluginConfig> pluginConfigRepository,
            IRepository<Plugin> pluginRepository,
            PluginServices plugins)
        {
            _pluginConfigRepository = pluginConfigRepository;
            _pluginRepository = pluginRepository;
            _pluginServices = plugins;
            SetCrumbs("Plugins", "Plugins");
        }

        public ActionResult Index()
        {
            string[] pluginFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"), "*.dll");

            List<Plugin> plugins = _pluginServices.GetPluginsInFolder(Server.MapPath("~/Plugins"));

            return View(plugins);
        }

        public ActionResult Install(string AssemblyName)
        {
            var plugins = _pluginServices.GetPluginsInFolder(Server.MapPath("~/Plugins"));

            IPluginDetails details = _pluginServices.GetPluginDetailsFromAssembly(Path.Combine(Server.MapPath("~/Plugins"), AssemblyName + ".dll"));

            ViewData["AssemblyName"] = AssemblyName;

            var results = TempData["InstallResults"];

            ViewData["InstallResults"] = results;

            return View(details);
        }

        [HttpPost]
        public ActionResult InstallPlugin(string AssemblyName)
        {
            IPluginDetails details = _pluginServices.GetPluginDetailsFromAssembly(Path.Combine(Server.MapPath("~/Plugins"), AssemblyName + ".dll"));

            TempData["PluginDetails"] = details;

            PluginInstall results = new PluginInstall { Configs = "", SQL = "" };

            if (!string.IsNullOrWhiteSpace(details.InstallDetails.GetSQL))
            {
                try
                {
                    SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["cleanConnectionString"].ConnectionString);
                    SqlCommand cmd = sqlCon.CreateCommand();
                    cmd.CommandText = details.InstallDetails.GetSQL;
                    sqlCon.Open();
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                }
                catch (Exception ex)
                {
                    results.SQL = ex.Message;
                }
            }

            var configs = details.InstallDetails.GetConfigs();

            if (configs != null && configs.Count > 01)
            {
                try
                {
                    foreach (var config in configs)
                    {
                        _pluginConfigRepository.Add(new PluginConfig()
                        {
                            Name = config.Name,
                            Note = config.Note,
                            Options = config.Options,
                            Type = config.Type,
                            Value = config.Value,
                            PluginGroup = details.Name
                        });
                    }
                }
                catch (Exception ex)
                {
                    results.Configs = ex.Message;
                }
            }

            if (results.Configs == "" && results.SQL == "")
            {
                _pluginRepository.Add(new Plugin
                {
                    Name = details.Name,
                    Description = details.Description,
                    Installed = true,
                    AssemblyName = AssemblyName
                });

                SetSuccess("Plugin successfully installed");
                return RedirectToAction("PluginInstalled", new { AssemblyName = AssemblyName });
            }
            else
            {
                SetError("An error occurred while installing the plugin");
                TempData["InstallResults"] = results;
                return RedirectToAction("Install", new { AssemblyName = AssemblyName });
            }
        }

        public ActionResult PluginInstalled(string AssemblyName)
        {
            IPluginDetails details = TempData["PluginDetails"] as IPluginDetails;

            ViewData["AssemblyName"] = AssemblyName;

            ViewData["PluginName"] = details.Name;

            return View();
        }

        [HttpPost]
        public ActionResult EditWebConfig(string AssemblyName, string PluginName)
        {
            try
            {
                var configuration = WebConfigurationManager.OpenWebConfiguration("~");
                var section = (SystemWebSectionGroup)configuration.GetSectionGroup("system.web");
                var info = new AssemblyInfo(AssemblyName);
                section.Compilation.Assemblies.Add(info);
                configuration.Save();
            }
            catch (Exception ex)
            {
                SetError("An error occurred while trying to edit the Web.config file");
                SetNotice(ex.Message);
                return RedirectToAction("Index");
            }

            ViewData["PluginName"] = PluginName;

            return View();
        }
    }
}