using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Services;
using mesoBoard.Web.Areas.Admin.Models;
using mesoBoard.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    // This needs reworking
    public class ConfigsController : BaseAdminController
    {
        private IRepository<Config> _configRepository;
        private IRepository<PluginConfig> _pluginConfigRepository;
        private IRepository<Role> _roleRepository;
        private IRepository<Theme> _themeRepository;
        private ThemeServices _themeServices;
        private Theme _currentTheme;
        private SiteConfig _siteConfig;

        public ConfigsController(
            IRepository<Config> configRepository,
            IRepository<PluginConfig> pluginConfigRepository,
            IRepository<Role> roleRepository,
            IRepository<Theme> themeRepository,
            ThemeServices themeServices,
            Theme currentTheme,
            SiteConfig siteConfig)
        {
            _configRepository = configRepository;
            _pluginConfigRepository = pluginConfigRepository;
            _roleRepository = roleRepository;
            _themeRepository = themeRepository;
            _themeServices = themeServices;
            _currentTheme = currentTheme;
            _siteConfig = siteConfig;
        }

        private class ConfigValidation
        {
            public bool Valid { get; set; }

            public string Error { get; set; }

            public string FieldError { get; set; }
        }

        private ConfigValidation ValidateConfig(Config config, string value)
        {
            var validation = new ConfigValidation()
            {
                Error = string.Empty,
                Valid = true
            };

            // With the exception of "int" and "string" config types, all other types have
            // values that are selected and not entered. "int" is the only one we need to verify
            switch (config.Type)
            {
                case "bool[]":
                case "bool":
                case "string[]":
                case "string":
                    break;

                case "int":
                    int intOut;
                    if (!int.TryParse(value, out intOut))
                    {
                        validation.Error = string.Format("{0} » {1} requires a valid number.", config.Group, config.Name);
                        validation.FieldError = "Enter a valid number.";
                        validation.Valid = false;
                    }
                    break;
            }

            return validation;
        }

        [HttpGet]
        public ActionResult Config()
        {
            SetBreadCrumb("Configuration");
            var configs = _configRepository.Get().Where(item => item.Name != "BoardCreateDate").ToList();

            IEnumerable<ConfigViewModel> configViewModels = configs.Select(item => new ConfigViewModel()
            {
                ConfigID = item.ConfigID,
                Group = item.Group,
                Name = item.Name,
                Note = item.Note,
                Type = item.Type,
                Value = item.Value,
                Options = item.Options
            });
            var themes = _themeRepository.Get().ToList();
            var roles = _roleRepository.Get().ToList();
            ConfigViewer model = new ConfigViewer()
            {
                ConfigGroups = configs.Select(item => item.Group).Distinct().ToArray(),
                Configs = configViewModels,
                ThemeList = new SelectList(themes, "ThemeID", "DisplayName", SiteConfig.BoardTheme),
                RolesList = new SelectList(roles, "RoleID", "Name", SiteConfig.RegistrationRole)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Config(FormCollection form)
        {
            int updated = 0;
            int errors = 0;

            foreach (string configKey in form.Keys)
            {
                Config config = _configRepository.First(item => item.Name.Equals(configKey));
                if (config != null)
                {
                    string value = Request.Form[configKey];
                    var modelState = new ModelStateDictionary();

                    if (value != config.Value)
                    {
                        if (string.IsNullOrEmpty(value))
                            ModelState.AddModelError(configKey, configKey + " requires a value");
                        else
                        {
                            var validation = ValidateConfig(config, value);
                            if (!validation.Valid)
                            {
                                modelState.AddModelError(value, validation.FieldError);
                                ModelState.AddModelError(string.Empty, validation.Error);
                                errors++;
                            }
                            else
                            {
                                if (config.Name == "BoardTheme")
                                    _themeServices.ChangeDefaultTheme(int.Parse(value));
                                else
                                {
                                    config.Value = value;
                                    _configRepository.Update(config);
                                }
                                updated++;
                            }
                        }
                    }
                    ModelState.Merge(modelState);
                }
            }

            if (updated != 0)
                SetSuccess(updated + " configs updated");

            if (errors != 0)
                SetError(errors + " config errors. See the messages below");
            
            _siteConfig.UpdateCache();
            Misc.ParseBBCodeScriptFile(_currentTheme);
            return RedirectToSelf();
        }

        [HttpPost]
        public ActionResult PluginConfig()
        {
            Dictionary<string, string> ToUpdate = new Dictionary<string, string>();
            foreach (string f in Request.Form.Keys)
            {
                PluginConfig config = _pluginConfigRepository.First(item => item.Name.Equals(f));
                if (config != null)
                {
                    if (string.IsNullOrEmpty(Request.Form[f]))
                        ModelState.AddModelError(f, f + " requires a value");
                    else if (config.Type == "int")
                    {
                        int intOut;
                        if (!int.TryParse(Request.Form[f], out intOut))
                            ModelState.AddModelError(f, string.Format("{0} » {1} requires a valid number", config.PluginGroup, config.Name));
                    }
                    ToUpdate.Add(f, Request.Form[f]);
                }
            }
            if (IsModelValidAndPersistErrors())
            {
                foreach (var update in ToUpdate)
                {
                    PluginConfig config = _pluginConfigRepository.First(item => item.Name.Equals(update.Key));
                    config.Value = update.Value;
                    _pluginConfigRepository.Update(config);
                }

                SetSuccess("PluginConfigs updated");
                return RedirectToAction("PluginConfig");
            }
            else
            {
                SetError("All fields are required, no values were updated");
                TempData["Errors"] = ModelState.Keys;
                return RedirectToAction("PluginConfig");
            }
        }

        [HttpGet]
        public ActionResult PluginConfig(string Plugin)
        {
            ViewData["Plugin"] = string.IsNullOrEmpty(Plugin) ? "" : Plugin;

            var errors = TempData["Errors"];
            if (errors != null)
                ViewData = (ViewDataDictionary)errors;
            SetBreadCrumb("Plugin Configs");

            var configs = _pluginConfigRepository.Get().ToList();
            var configGroups = configs.Select(item => item.PluginGroup).Distinct().ToArray();

            var model = new PluginConfigsViewModel()
            {
                ConfigGroups = configGroups,
                Configs = configs
            };
            return View(model);
        }
    }
}