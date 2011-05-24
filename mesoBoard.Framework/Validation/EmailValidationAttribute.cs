using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
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
