using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using mesoBoard.Services;

namespace mesoBoard.Framework.Validation
{
    public class EmailValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return RegEx.IsValidEmail((string)value);
        }
    }
}