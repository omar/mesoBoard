using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace mesoBoard.Framework.Core
{
    [InstalledCheck]
    [OfflineCheck]
    [TrackActivity]
    public abstract class BaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var timeZoneOffset = SiteConfig.TimeOffset.ToInt();
            ViewData[ViewDataKeys.TimeZoneOffset] = timeZoneOffset;      
            base.OnActionExecuted(filterContext);

            var result = filterContext.Result as ViewResult;
            
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
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
            var routeData = new RouteValueDictionary(routeValues);
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
