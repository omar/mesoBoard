using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Services;
using System.Web.Routing;

namespace mesoBoard.Framework
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class OfflineCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SiteConfig.Offline.BoolValue())
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];

                var actionAllowOffline = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowOfflineAttribute), false);
                var controllerAllowOffline = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowOfflineAttribute), true);
                if (actionAllowOffline.Length == 0 && controllerAllowOffline.Length == 0)
                {
                    RouteValueDictionary redirectRoute = new RouteValueDictionary(new { action = "Offline", controller = "Board", area = "" });
                    filterContext.Result = new RedirectToRouteResult(redirectRoute);
                }
            }
        }
    }
}