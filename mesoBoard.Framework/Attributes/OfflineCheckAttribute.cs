using System;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

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

                var actionDescriptor = (filterContext.ActionDescriptor as ControllerActionDescriptor);
                var actionAllowOffline = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowOfflineAttribute), false);
                var controllerAllowOffline = actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AllowOfflineAttribute), true);
                if (actionAllowOffline.Length == 0 && controllerAllowOffline.Length == 0)
                {
                    var redirectRoute = new RouteValueDictionary(new { action = "Offline", controller = "Board", area = "" });
                    filterContext.Result = new RedirectToRouteResult(redirectRoute);
                }
            }
        }
    }
}