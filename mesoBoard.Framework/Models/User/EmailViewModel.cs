using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using mesoBoard.Services;
using mesoBoard.Framework.Validation;

namespace mesoBoard.Framework.Models
{
    public class EmailViewModel
    {
        public string CurrentEmail { get; set; }

        [Required(ErrorMessage = "Enter a valid email address.")]
        [EmailValidation(ErrorMessage="Enter a valid email address.")]
        public string NewEmail { get; set; }
    }
}