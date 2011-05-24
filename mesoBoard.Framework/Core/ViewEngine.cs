using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace mesoBoard.Framework.Core
{
    //
    //
    // Copyright, 2010 dimebrain
    // http://dimebrain.com/2009/11/a-themes-engine-for-asp-net-mvc-2.html
    // Licensed under the MIT License - http://www.opensource.org/licenses/mit-license.php
    //
    // Modified for mesoBoard
    //


    /// <summary>
    /// Provides a flexible <see cref="WebFormViewEngine" /> for adding theming capabilities to your views.
    /// You have the option of using a theme to override only what you need, whether that's CSS, Javascript,
    /// MasterPages, or specific views. This way both look and feel as well as site structure may be
    /// changed on demand.
    /// </summary>
    public class ViewEngine : RazorViewEngine
    {
        // format is ":ViewCacheEntry:{cacheType}:{prefix}:{name}:{controllerName}:{areaName}:{themeName}"
        private const string CacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:{5}";
        private const string CacheKeyPrefixMaster = "Master";
        private const string CacheKeyPrefixPartial = "Partial";
        private const string CacheKeyPrefixView = "View";

        private static readonly string[] _emptyLocations = new string[0];

        public string Theme { get; private set; }

        // {0}name:{1}controllerName:{2}areaName:{3}themeName
        public ViewEngine()
        {
            AreaMasterLocationFormats = new[] {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Views/Areas/{2}/{1}/{0}.cshtml",
                "~/Views/Areas/{2}/Shared/{0}.cshtml",
            };

            AreaViewLocationFormats = new[] {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
            };

            MasterLocationFormats = new[] {
                  "~/Themes/{3}/Views/{1}/{0}.cshtml",
                  "~/Themes/{3}/Views/Shared/{0}.cshtml",
                  "~/Views/{1}/{0}.cshtml",
                  "~/Views/Shared/{0}.cshtml",
                  "~/Plugins/{3}/Views/Shared/{0}.cshtml",
            };

            ViewLocationFormats = new[] {
                "~/Themes/{3}/Views/{1}/{0}.cshtml",
                "~/Themes/{3}/Views/Shared/{0}.cshtml",
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Plugins/{4}/Views/{1}/{0}.cshtml",
                "~/Plugins/{4}/Views/Shared/{0}.cshtml",
            };


            PartialViewLocationFormats = ViewLocationFormats;
            AreaPartialViewLocationFormats = AreaViewLocationFormats;
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("Argument cannot be null or empty", "viewName");
            }

            SetTheme(controllerContext);
            
            string[] viewLocationsSearched;
            string[] masterLocationsSearched;

            var controllerName = controllerContext.RouteData.GetRequiredString("controller");

            var viewPath = GetPath(controllerContext, ViewLocationFormats, AreaViewLocationFormats,
                                   "ViewLocationFormats", viewName, controllerName, CacheKeyPrefixView, useCache,
                                   out viewLocationsSearched);

            var masterPath = GetPath(controllerContext, MasterLocationFormats, AreaMasterLocationFormats,
                                     "MasterLocationFormats", masterName, controllerName, CacheKeyPrefixMaster,
                                     useCache, out masterLocationsSearched);

            if (String.IsNullOrEmpty(viewPath) ||
                (String.IsNullOrEmpty(masterPath) && !String.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
            }

            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
        }

        private void SetTheme(ControllerContext controllerContext)
        {
            var context = controllerContext.RequestContext.HttpContext;
            this.Theme = (string)controllerContext.HttpContext.Items[HttpContextItemKeys.ThemeFolder];
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("Argument cannot be null or empty", "partialViewName");
            }

            SetTheme(controllerContext);

            string[] searched;
            var controllerName = controllerContext.RouteData.GetRequiredString("controller");
            var partialPath = GetPath(controllerContext, PartialViewLocationFormats, AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, controllerName, CacheKeyPrefixPartial, useCache, out searched);

            return String.IsNullOrEmpty(partialPath)
                       ? new ViewEngineResult(searched)
                       : new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);
        }

        private string GetPath(ControllerContext controllerContext, IEnumerable<string> locations, string[] areaLocations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;

            if (String.IsNullOrEmpty(name))
            {
                return String.Empty;
            }

            var areaName = controllerContext.RouteData.GetAreaName();
            var usingAreas = !String.IsNullOrEmpty(areaName);
            var viewLocations = GetViewLocations(locations, (usingAreas) ? areaLocations : null);
            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                                                                  "{0} cannot be null or empty.", locationsPropertyName));
            }

            var nameRepresentsPath = IsSpecificPath(name);
            var cacheKey = CreateCacheKey(cacheKeyPrefix, name, (nameRepresentsPath) ? String.Empty : controllerName, areaName);
            if (useCache)
            {
                return ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
            }

            return (nameRepresentsPath) ?
                                            GetPathFromSpecificName(controllerContext, name, cacheKey, ref searchedLocations) :
                                                                                                                                  GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, cacheKey, ref searchedLocations);
        }

        private string CreateCacheKey(string prefix, string name, string controllerName, string areaName)
        {
            return String.Format(CultureInfo.InvariantCulture, CacheKeyFormat,
                                 GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName, Theme);
        }

        private string GetPathFromGeneralName(ControllerContext controllerContext, IList<ViewLocation> locations, string name, string controllerName, string areaName, string cacheKey, ref string[] searchedLocations)
        {
            var result = String.Empty;
            searchedLocations = new string[locations.Count];

            string pluginFolder = string.Empty;

            if (controllerContext.HttpContext.Items.Contains(HttpContextItemKeys.PluginFolder))
                pluginFolder = (string)controllerContext.HttpContext.Items[HttpContextItemKeys.PluginFolder];
                

            for (var i = 0; i < locations.Count; i++)
            {
                var location = locations[i];
                var virtualPath = "";
                virtualPath = location.Format(name, controllerName, areaName, Theme, pluginFolder);                

                if (FileExists(controllerContext, virtualPath))
                {
                    searchedLocations = _emptyLocations;
                    result = virtualPath;
                    ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
                    break;
                }

                searchedLocations[i] = virtualPath;
            }

            return result;
        }

        private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            var result = name;

            if (!FileExists(controllerContext, name))
            {
                result = String.Empty;
                searchedLocations = new[] { name };
            }

            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
            return result;
        }

        private static List<ViewLocation> GetViewLocations(IEnumerable<string> viewLocationFormats,
                                                           IEnumerable<string> areaViewLocationFormats)
        {
            var allLocations = new List<ViewLocation>();

            if (areaViewLocationFormats != null)
            {
                foreach (var areaViewLocationFormat in areaViewLocationFormats)
                {
                    allLocations.Add(new AreaAwareViewLocation(areaViewLocationFormat));
                }
            }

            if (viewLocationFormats != null)
            {
                foreach (var viewLocationFormat in viewLocationFormats)
                {
                    allLocations.Add(new ViewLocation(viewLocationFormat));
                }
            }

            return allLocations;
        }

        private static bool IsSpecificPath(string name)
        {
            var c = name[0];
            return (c == '~' || c == '/');
        }

        private class ViewLocation
        {
            protected readonly string _virtualPathFormatString;

            public ViewLocation(string virtualPathFormatString)
            {
                _virtualPathFormatString = virtualPathFormatString;
            }

            public virtual string Format(string viewName, string controllerName, string areaName, string themeName, string pluginName = "")
            {
                var result = String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName, themeName, pluginName);
                return result;
            }
        }

        private class AreaAwareViewLocation : ViewLocation
        {
            public AreaAwareViewLocation(string virtualPathFormatString)
                : base(virtualPathFormatString)
            {

            }

            public override string Format(string viewName, string controllerName, string areaName, string themeName, string pluginName = "")
            {
                var result = String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName, themeName, pluginName);
                return result;
            }
        }
    }

    public static class RoutingExtensions
    {
        public static string GetAreaName(this RouteBase route)
        {
            var routeWithArea = route as IRouteWithArea;
            if (routeWithArea != null)
            {
                return routeWithArea.Area;
            }

            var castRoute = route as Route;
            if (castRoute != null && castRoute.DataTokens != null)
            {
                return castRoute.DataTokens["area"] as string;
            }

            return null;
        }

        public static string GetAreaName(this RouteData routeData)
        {
            object area;
            if (routeData.DataTokens.TryGetValue("area", out area))
            {
                return area as string;
            }

            return GetAreaName(routeData.Route);
        }
    }

}