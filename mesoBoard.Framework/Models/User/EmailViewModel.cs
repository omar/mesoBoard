using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using mesoBoard.Framework.Validation;
using mesoBoard.Services;

namespace mesoBoard.Framework.Models
{
    public class EmailViewModel
    {
        public string CurrentEmail { get; set; }

        [Required(ErrorMessage = "Enter a valid email address.")]
        [EmailValidation(ErrorMessage = "Enter a valid email address.")]
        public string NewEmail { get; set; }
    }
}