using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace mesoBoard.Framework
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class InstalledCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controller = (string)filterContext.RouteData.Values["controller"];
            if (!Settings.IsInstalled && !string.IsNullOrWhiteSpace(controller) && !controller.Equals("Install", System.StringComparison.InvariantCultureIgnoreCase))
            {
                var redirectRoute = new RouteValueDictionary(new { controller = "Install", area = "" });
                filterContext.Result = new RedirectToRouteResult(redirectRoute);
            }
        }
    }
}