using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using mesoBoard.Services;

namespace mesoBoard.Web.Areas.Admin.ViewModels 
{
    public class RankViewModel : IValidatableObject
    {
        public int RankID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool IsRoleRank { get; set; }

        public int PostCount { get; set; }

        public string Image { get; set; }

        [RegularExpression(RegEx.HexColorPattern, ErrorMessage = "Enter a valid hex color code.")]
        public string Color { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsRoleRank && PostCount == 0)
                yield return new ValidationResult("A post count is required for non-role ranks.", new[] { "PostCount" });

        }
    }
}