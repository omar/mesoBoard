using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Services;
using Ninject;
using Ninject.Infrastructure;
using Ninject.Modules;
using Ninject.Web.Mvc;

namespace mesoBoard.Web
{
    public class MvcApplication : HttpApplication, IHaveKernel
    {
        public static IKernel Kernel { get; set; }

        IKernel IHaveKernel.Kernel
        {
            get { return Kernel; }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            AreaRegistration.RegisterAllAreas();

            routes.MapRoute(
                RouteKeys.DefaultRouteName,                                                           // Route name
                RouteKeys.DefaultRouteUrl,                                                            // URL with parameters
                new { controller = "Board", action = "Index", id = UrlParameter.Optional }            // Parameter defaults
            );

            StaticResources.Initialize(routes);
        }

        protected void Application_Start()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            if (Settings.IsInstalled)
            {
                SiteConfig.UpdateCache();

                //LoadPlugins();
            }
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionFormat = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            HttpContext.Current.Cache["mesoBoardVersion"] = versionFormat;
        }

        private void LoadPlugins()
        {
            /* Plugin system is not ready yet */
            string pluginsPath = Server.MapPath("~/Plugins");
            string currentShadowCopyDirectories = AppDomain.CurrentDomain.SetupInformation.ShadowCopyDirectories;

            // SetShadowCopyPath() doesn't work in medium trust need
            // to find another method of making sure .DLLs aren't locked
#pragma warning disable 0618

            // Shadow copies all plugin assemblies to allow overwriting of .DLL files
            AppDomain.CurrentDomain.SetShadowCopyPath(currentShadowCopyDirectories + ";" + pluginsPath);
#pragma warning restore 0618

            string[] pluginFiles = Directory.GetFiles(pluginsPath, "*.dll");
            var assemblies = pluginFiles.Select(item => Assembly.LoadFrom(item));
            Kernel.Load(assemblies);

            var pluginRepository = Kernel.Get<IRepository<Plugin>>();

            RouteValueDictionary defaultRoute = null;
            List<RouteBase> routeCollection = new List<RouteBase>();

            foreach (var plugin in pluginRepository.Get().ToList())
            {
                var pluginDetails = Kernel.TryGet<IPluginDetails>(plugin.Name);
                if (pluginDetails != null)
                {
                    if (pluginDetails.DefaultRoute != null)
                    {
                        defaultRoute = pluginDetails.DefaultRoute;
                    }
                    routeCollection.AddRange(pluginDetails.Routes);
                }
            }

            if (defaultRoute != null)
                StaticResources.OverrideDefaultRoute(defaultRoute);

            if (routeCollection.Count > 0)
                StaticResources.Insert(routeCollection);
        }

        public void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));

            if (routeData != null)
            {
                string controllerName = routeData.GetRequiredString("controller");
                if (!Settings.IsInstalled && !string.IsNullOrWhiteSpace(controllerName) && !controllerName.Equals("Install", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    Response.Redirect("~/Install");
                    return;
                }

                if (Settings.IsInstalled)
                {
                    var themeService = Kernel.Get<ThemeServices>();
                    var theme = themeService.GetDefaultTheme();
                    HttpContext.Current.Items[HttpContextItemKeys.ThemeFolder] = theme.FolderName;
                    HttpContext.Current.Items[HttpContextItemKeys.CurrentTheme] = theme;
                }
            }
        }

        public void Session_Start(object sender, EventArgs e)
        {
            // Need to add something to session to ensure that Session_End fires
            Session[SessionKeys.LastActivityUpdate] = DateTime.UtcNow.AddMinutes(-16);
            Session[SessionKeys.IPAddress] = Request.UserHostAddress;
        }

        public void Session_End(object sender, EventArgs e)
        {
            if (Settings.IsInstalled)
            {
                var onlineUsers = Kernel.Get<IRepository<OnlineUser>>();
                var onlineGuests = Kernel.Get<IRepository<OnlineGuest>>();

                if (Session[SessionKeys.UserID] != null)
                {
                    var userServices = Kernel.Get<UserServices>();
                    int userID = (int)Session[SessionKeys.UserID];
                    userServices.LogoutRoutine(userID);
                }
                string ipAddress = (string)Session[SessionKeys.IPAddress];
                var guest = onlineGuests.First(item => item.IP == ipAddress);

                if (guest != null)
                    onlineGuests.Delete(guest.OnlineGuestID);
            }
        }
    }
}