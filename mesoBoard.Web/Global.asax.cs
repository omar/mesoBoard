using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using mesoBoard.Framework.Core;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System.Collections.Generic;

namespace mesoBoard.Web
{
    public class MvcApplication : NinjectHttpApplication
    {
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

        protected override void OnApplicationStarted()
        {
            RegisterRoutes(RouteTable.Routes);
            RegisterGlobalFilters(GlobalFilters.Filters);
            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(new ViewEngine());
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(this.Kernel));

            string conn = Settings.EntityConnectionString;

            ServiceLocator.Initialize(this.Kernel);

            if (Settings.IsInstalled)
            {
                SiteConfig.UpdateCache();

                var globalServices = Kernel.Get<GlobalServices>();
                globalServices.PruneOnlineGuests();
                globalServices.PruneOnlineUsers();
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
            ServiceLocator.Kernel.Load(assemblies);

            var pluginRepository = Kernel.Get<IRepository<Plugin>>();

            RouteValueDictionary defaultRoute = null;
            List<RouteBase> routeCollection = new List<RouteBase>();

            foreach (var plugin in pluginRepository.Get())
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

        public void Application_BeginRequest(object sender, EventArgs e)
        {
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

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            NinjectModule[] modules = new NinjectModule[]
            {
                new WebModule()
            };

            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Load(modules);
            return kernel;
        }
    }
}