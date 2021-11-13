using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace mesoBoard.Framework.Validation
{
    public class CaptchaValidationAttribute : ValidationAttribute
    {
        private string _formFieldName = "Captcha";
        public new string ErrorMessage = "Image verification incorrect";

        public CaptchaValidationAttribute(string formFieldName = "Captcha")
        {
            _formFieldName = formFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpContext = validationContext.GetService<IHttpContextAccessor>().HttpContext;

            var error = new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
            if (httpContext == null ||
                httpContext.Session == null ||
                httpContext.Request == null ||
                httpContext.Request.Form == null)
                return error;
            string captchaFormFieldName = _formFieldName;
            string guidFieldName = "captcha_code";
            string attemptedValue = httpContext.Request.Form[captchaFormFieldName];
            string guid = httpContext.Request.Form[guidFieldName];

            string solution = (string)httpContext.Session.GetString(SessionKeys.CaptchaSessionPrefix + guid);

            httpContext.Session.Remove(SessionKeys.CaptchaSessionPrefix + guid);
            if ((solution == null) || (attemptedValue != solution))
                return error;
            else
                return ValidationResult.Success;
        }
    }
}