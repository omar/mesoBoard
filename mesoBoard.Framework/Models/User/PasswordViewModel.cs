using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mesoBoard.Services;

namespace mesoBoard.Framework.Models
{
    public class PasswordViewModel : IValidatableObject
    {
        public int MinimumPasswordLength { get; set; }

        [Required]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int passwordMinimumLength = SiteConfig.PasswordMin.ToInt();
            if (NewPassword.Length < passwordMinimumLength)
            {
                string errorMessage = "Password must be at least {0} characters long";
                yield return new ValidationResult(string.Format(errorMessage, passwordMinimumLength), new[] { "NewPassword" });
            }
        }
    }
}