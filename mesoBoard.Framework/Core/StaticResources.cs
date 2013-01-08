using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using mesoBoard.Common;

namespace mesoBoard.Framework.Core
{
    public static class StaticResources
    {
        private static RouteCollection _routes;

        public static void Initialize(RouteCollection routes)
        {
            _routes = routes;
        }

        public static void OverrideDefaultRoute(RouteValueDictionary routeValues)
        {
            _routes.RemoveAt(2);

            Route route = new Route(RouteKeys.DefaultRouteUrl, new MvcRouteHandler())
            {
                Defaults = routeValues
            };

            _routes.Add(RouteKeys.DefaultRouteUrl, route);
        }

        public static void Insert(IEnumerable<RouteBase> routes)
        {
            foreach (var route in routes)
                _routes.Insert(2, route);
        }
    }
}