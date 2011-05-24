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
        public Theme CurrentTheme { get; set; }
        public User CurrentUser { get; private set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var result = filterContext.Result as ViewResultBase;
            if (result != null)
            {
                var model = filterContext.Controller.ViewData.Model as BaseViewModel;
                
                if (model != null)
                {
                    model.CurrentTheme = CurrentTheme;
                    model.CurrentUser = CurrentUser;
                    model.IsAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
                }
            }

            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!Settings.IsInstalled)
                return;

            var userServices = ServiceLocator.Get<UserServices>();
            var themeServices = ServiceLocator.Get<ThemeServices>();
            var globalServices = ServiceLocator.Get<GlobalServices>();

            bool isAuthenticated = requestContext.HttpContext.User.Identity.IsAuthenticated;
            string controllerName = requestContext.RouteData.GetRequiredString("controller");
            string ipAddress = requestContext.HttpContext.Request.UserHostAddress;

            var timeZoneOffset = SiteConfig.TimeOffset.ToInt();
            ViewData[ViewDataKeys.TimeZoneOffset] = timeZoneOffset;

            if (isAuthenticated)
            {
                this.CurrentUser = userServices.GetUser(int.Parse(requestContext.HttpContext.User.Identity.Name));
                if (CurrentUser == null)
                {
                    this.CurrentUser = new Data.User { UserID = 0 };
                    FormsAuthentication.SignOut();
                    isAuthenticated = false;
                }
                else
                {
                    ViewData[ViewDataKeys.CurrentUser] = CurrentUser;
                }
            }
            else
                this.CurrentUser = new Data.User { UserID = 0 };

            
            string previewTheme = (string)requestContext.HttpContext.Session["ptheme"];

            if (requestContext.RouteData.GetAreaName() == "Admin")
                CurrentTheme = themeServices.GetAdminTheme();
            else
                CurrentTheme = themeServices.GetTheme(CurrentUser, controllerName, previewTheme);

            requestContext.HttpContext.Items[HttpContextItemKeys.ThemeFolder] = CurrentTheme.FolderName;
        }

        public void SetError(string msg)
        {
            ViewData[ViewDataKeys.GlobalMessages.Error] = msg;
            TempData[ViewDataKeys.GlobalMessages.Error] = msg;
        }

        public void SetSuccess(string msg)
        {
            TempData[ViewDataKeys.GlobalMessages.Success] = msg;
            ViewData[ViewDataKeys.GlobalMessages.Success] = msg;
        }

        public void SetNotice(string msg)
        {
            TempData[ViewDataKeys.GlobalMessages.Notice] = msg;
            ViewData[ViewDataKeys.GlobalMessages.Notice] = msg;
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
