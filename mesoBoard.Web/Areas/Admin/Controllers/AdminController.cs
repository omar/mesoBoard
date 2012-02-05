using System.Collections.Generic;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Web.Areas.Admin.Models;
using System.IO;
using System.Reflection;
using System;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public partial class AdminController : BaseAdminController
    {
        IRepository<User> _userRepository;
        IRepository<ReportedPost> _reportedPostRepository;
        IRepository<Plugin> _pluginRepository;

        public AdminController(
            IRepository<User> userRepository,
            IRepository<ReportedPost> reportedPostRepository,
            IRepository<Plugin> pluginRepository)
        {
            _userRepository = userRepository;
            _reportedPostRepository = reportedPostRepository;
            _pluginRepository = pluginRepository;
        }

        public ActionResult Index()
        {
            ViewData["NewestRegistrations"] = _userRepository.Get();
            ViewData["ReportedPosts"] = _reportedPostRepository.Get();
            SetCrumb("Summary");
            return View();
        }

        public ActionResult ReportedPosts()
        {
            SetCrumb("Reported Posts");
            IEnumerable<ReportedPost> reported = _reportedPostRepository.Get();
            return View(reported);
        }

        public ActionResult MarkAsSafe(int ReportedPostID)
        {
            _reportedPostRepository.Delete(ReportedPostID);
            SetSuccess("Post marked as safe");
            return RedirectToAction("ReportedPosts");
        }

        public ActionResult Confirm(string YesRedirect, string NoRedirect)
        {
            ViewData["YesUrl"] = YesRedirect;
            ViewData["NoUrl"] = NoRedirect;
            return View();
        }

        [ChildActionOnly]
        public ActionResult PluginsMenu()
        {
            List<PluginMenu> menus = new List<PluginMenu>();
            string[] pluginFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"), "*.dll");

            foreach (string plugin in pluginFiles)
            {
                Assembly loadPlugin = Assembly.LoadFrom(plugin);
                string name = loadPlugin.GetName().Name;
                Type toLoad = loadPlugin.GetType(name + ".PluginDetails");
                IPluginDetails pluginDetails = Activator.CreateInstance(toLoad) as IPluginDetails;

                if (_pluginRepository.First(item => item.Name.Equals(pluginDetails.Name)) == null)
                    continue;

                menus.Add(new PluginMenu
                {
                    MainLink = pluginDetails.AdminMenu.ParentLink,
                    ChildLinks = pluginDetails.AdminMenu.ChildLinks
                });
            }

            return View(menus);
        }
    }
}