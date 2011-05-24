using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
                RouteValueDictionary redirectRoute = new RouteValueDictionary(new { controller = "Install", area = "" });
                filterContext.Result = new RedirectToRouteResult(redirectRoute);
            }
        }
    }
}