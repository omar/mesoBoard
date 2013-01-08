using System.Web.Mvc;

namespace mesoBoard.Framework
{
    public class CaptchaVerifyAttribute : ActionFilterAttribute
    {
        private string CaptchaFormFieldName;
        private string CaptchaExtensionMethodParameter;
        public string ErrorMessage = "Image verification incorrect";

        public CaptchaVerifyAttribute(string CaptchaFormFieldName, string CaptchaExtensionMethodParameter)
        {
            this.CaptchaFormFieldName = CaptchaFormFieldName;
            this.CaptchaExtensionMethodParameter = CaptchaExtensionMethodParameter;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string attemptedValue = filterContext.HttpContext.Request.Form[CaptchaFormFieldName];
            string guid = filterContext.HttpContext.Request.Form[CaptchaExtensionMethodParameter];

            string solution = (string)filterContext.HttpContext.Session[SessionKeys.CaptchaSessionPrefix + guid];

            filterContext.HttpContext.Session.Remove(SessionKeys.CaptchaSessionPrefix + guid);
            if ((solution == null) || (attemptedValue != solution))
                filterContext.Controller.ViewData.ModelState.AddModelError(CaptchaFormFieldName, ErrorMessage);
        }
    }
}