using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

            byte[] solutionBytes = null;
            
            filterContext.HttpContext.Session.TryGetValue((SessionKeys.CaptchaSessionPrefix + guid), out solutionBytes);

            var solution = Encoding.ASCII.GetString(solutionBytes);

            filterContext.HttpContext.Session.Remove(SessionKeys.CaptchaSessionPrefix + guid);
            if ((solution == null) || (attemptedValue != solution))
                ((Controller)filterContext.Controller).ViewData.ModelState.AddModelError(CaptchaFormFieldName, ErrorMessage);
        }
    }
}