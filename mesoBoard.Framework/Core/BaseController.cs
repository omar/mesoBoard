using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using mesoBoard.Data;
using mesoBoard.Services;
using mesoBoard.Framework.Models;
using System.Web.Routing;
using System.Collections.Generic;

namespace mesoBoard.Framework.Core
{
    [InstalledCheck]
    [OfflineCheck]
    [TrackActivity]
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var result = filterContext.Result as ViewResultBase;
            
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            
            if (!Settings.IsInstalled)
                return;

            var timeZoneOffset = SiteConfig.TimeOffset.ToInt();
            ViewData[ViewDataKeys.TimeZoneOffset] = timeZoneOffset;           
        }

        public void SetError(string message)
        {
            SetMessage(ViewDataKeys.GlobalMessages.Error, message);
        }

        public void SetSuccess(string message)
        {
            SetMessage(ViewDataKeys.GlobalMessages.Success, message);
        }

        public void SetNotice(string message)
        {
            SetMessage(ViewDataKeys.GlobalMessages.Notice, message);
        }

        private void SetMessage(string key, string message)
        {
            TempData[key] = message;
            ViewData[key] = message;
        }

        public void SetCrumbs(string BreadCrumb, string TopBreadCrumb)
        {
            SetTopBreadCrumb(TopBreadCrumb);
            SetBreadCrumb(BreadCrumb);
        }

        public void SetCrumb(string BreadCrumb)
        {
            SetBreadCrumb(BreadCrumb);
        }

        public void SetBreadCrumb(string BreadCrumb)
        {
            ViewData[ViewDataKeys.BreadCrumb] = BreadCrumb;
        }

        public void SetTopBreadCrumb(string TopBreadCrumb)
        {
            ViewData[ViewDataKeys.TopBreadCrumb] = TopBreadCrumb;
        }

        protected RedirectToRouteResult RedirectToSelf(object routeValues)
        {
            RouteValueDictionary routeData = new RouteValueDictionary(routeValues);
            string controller = (string)this.ControllerContext.RouteData.Values["controller"];
            string action = (string)this.ControllerContext.RouteData.Values["action"];
            routeData.Add("controller", controller);
            routeData.Add("action", action);
            
            return RedirectToRoute(routeData);
        }

        protected RedirectToRouteResult RedirectToSelf()
        {
            return RedirectToSelf(null);
        }

        protected void PersistModelState()
        {
            TempData["ModelState"] = ModelState;
        }

        protected bool IsModelValidAndPersistErrors()
        {
            PersistModelState();
            return ModelState.IsValid;
        }
    }
}
