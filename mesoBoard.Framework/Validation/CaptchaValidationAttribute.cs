using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
            var error = new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
            if (HttpContext.Current == null ||
                HttpContext.Current.Session == null ||
                HttpContext.Current.Request == null ||
                HttpContext.Current.Request.Form == null)
                return error;
            string captchaFormFieldName = _formFieldName;
            string guidFieldName = "captcha_code";
            string attemptedValue = HttpContext.Current.Request.Form[captchaFormFieldName];
            string guid = HttpContext.Current.Request.Form[guidFieldName];

            string solution = (string)HttpContext.Current.Session[SessionKeys.CaptchaSessionPrefix + guid];

            HttpContext.Current.Session.Remove(SessionKeys.CaptchaSessionPrefix + guid);
            if ((solution == null) || (attemptedValue != solution))
                return error;
            else
                return ValidationResult.Success;
        }
    }
}