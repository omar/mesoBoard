using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using mesoBoard.Services;
using mesoBoard.Framework.Core;
using System.ComponentModel;
using mesoBoard.Framework.Validation;
using System.Web.Mvc;

namespace mesoBoard.Framework.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailValidation(ErrorMessage= "Enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        public string Password {get; set;}

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [CaptchaValidation]
        [Display(Name = "Image Verification")]
        public string Captcha { get; set; }

        [Required]
        [Display(Name = "Terms and Conditions")]
        [RegularExpression("[T|t]rue", ErrorMessage = "You must accept the terms and conditions.")]
        public bool Terms { get; set; }
    }
}