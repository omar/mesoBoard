using System.Collections.Generic;
using System.Linq;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace mesoBoard.Framework.Core
{
    [InstalledCheck]
    [OfflineCheck]
    [TrackActivity]
    [SetTempDataModelStateAttribute]
    [RestoreModelStateFromTempData]
    public abstract class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var themeServices = context.HttpContext.RequestServices.GetService<ThemeServices>();
            var theme = themeServices.GetDefaultTheme();
            context.HttpContext.Items[HttpContextItemKeys.ThemeFolder] = theme.FolderName;
            context.HttpContext.Items[HttpContextItemKeys.CurrentTheme] = theme;

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var timeZoneOffset = SiteConfig.TimeOffset.ToInt();
            ViewData[ViewDataKeys.TimeZoneOffset] = timeZoneOffset;      
            base.OnActionExecuted(filterContext);

            // var result = filterContext.Result as ViewResult;

            // if (TempData["ModelState"] != null)
            // {
            //     var storedModelState = JsonConvert.DeserializeObject<ModelStateDictionary>(TempData["ModelState"] as string);
            //     if (!ModelState.Equals(storedModelState))
            //     {
            //         ModelState.Merge(storedModelState);
            //     }
            // }
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

        // protected void PersistModelState()
        // {
        //     TempData["ModelState"] = JsonConvert.SerializeObject(ModelState);
        // }

        protected bool IsModelValidAndPersistErrors()
        {
            // PersistModelState();
            return ModelState.IsValid;
        }
    }

    public class SetTempDataModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var controller = filterContext.Controller as Controller;
            var modelState = controller?.ViewData.ModelState;
            if (modelState != null)
            {
                var listError = modelState.Where(x => x.Value.Errors.Any())
                    .ToDictionary(m => m.Key, m => m.Value.Errors
                    .Select(s => s.ErrorMessage)
                    .FirstOrDefault(s => s != null));
                controller.TempData["ModelState"] = JsonConvert.SerializeObject(listError);
            }
        }
    }
    public class RestoreModelStateFromTempDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var controller = filterContext.Controller as Controller;
            var tempData = controller?.TempData?.Keys;
            if (controller != null && tempData != null)
            {
                if (tempData.Contains("ModelState"))
                {
                    var modelStateString = controller.TempData["ModelState"].ToString();
                    var listError = JsonConvert.DeserializeObject<Dictionary<string, string>>(modelStateString);
                    var modelState = new ModelStateDictionary();
                    foreach (var item in listError)
                    {
                        modelState.AddModelError(item.Key, item.Value ?? "");
                    }

                    controller.ViewData.ModelState.Merge(modelState);
                }
            }
        }
    }
}
