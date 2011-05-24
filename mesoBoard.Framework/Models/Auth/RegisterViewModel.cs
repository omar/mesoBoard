using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using mesoBoard.Services;
using mesoBoard.Framework.Core;
using System.ComponentModel;
using mesoBoard.Framework.Validation;

namespace mesoBoard.Framework.Models
{
    [PropertiesMustMatch("Password", "ConfirmPassword")]
    public class RegisterViewModel : IValidatableObject
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
        public string ConfirmPassword { get; set; }

        [Required]
        [CaptchaValidation]
        [Display(Name = "Image Verification")]
        public string Captcha { get; set; }

        [Required]
        [Display(Name = "Terms and Conditions")]
        [RegularExpression("[T|t]rue", ErrorMessage = "You must accept the terms and conditions.")]
        public bool Terms { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var userServices = ServiceLocator.Get<UserServices>();
            int usernameMin = SiteConfig.UsernameMin.ToInt();
            int usernameMax = SiteConfig.UsernameMax.ToInt();
            int passwordMinimum = SiteConfig.PasswordMin.ToInt();

            if (Username.Length > usernameMax || Username.Length < usernameMin)
            {
                string message = string.Format("Username length must be between {0} and {1} characters long", usernameMin, usernameMax);
                yield return new ValidationResult("Username length must be between " + usernameMin + " and " + usernameMax + " characters long", new[] { "Username" });
            }
            else if (userServices.GetUser(Username) != null)
                yield return new ValidationResult("Username already taken", new[] { "Username" });

            if (userServices.EmailInUse(Email))
                yield return new ValidationResult("Email is already in use by another user", new[] { "Email" });

            if(Password.Length < passwordMinimum)
            {
                string message = string.Format("Password must be at least {0}", passwordMinimum);
                yield return new ValidationResult(message, new[] { "Password" });
            }
        }
    }
}