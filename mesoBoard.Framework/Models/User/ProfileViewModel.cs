using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Data;
using mesoBoard.Framework.Validation;
using mesoBoard.Services;

namespace mesoBoard.Framework.Models
{
    public class ProfileViewModel : IValidatableObject
    {
        public string Location { get; set; }

        public string AIM { get; set; }

        public int? ICQ { get; set; }

        [EmailValidation(ErrorMessage = "Enter a valid email address.")]
        public string MSN { get; set; }

        public string Website { get; set; }

        public bool AlwaysShowSignature { get; set; }

        public bool AlwaysSubscribeToThread { get; set; }

        public bool CanSelectTheme { get; set; }

        public int? ThemeID { get; set; }

        public IEnumerable<Theme> Themes { get; set; }

        public SelectList ThemeList
        {
            get
            {
                return new SelectList(Themes, "ThemeID", "DisplayName");
            }
        }

        public IEnumerable<Role> RankRoles { get; set; }

        public int? DefaultRankRole { get; set; }

        public DateTime? Birthdate { get; set; }

        public int? Day { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public SelectList DayList { get { return new SelectList(Enumerable.Range(1, 31)); } }

        public SelectList MonthList { get { return new SelectList(Enumerable.Range(1, 12)); } }

        public SelectList YearList { get { return new SelectList(Enumerable.Range(DateTime.UtcNow.Year - 100, 100)); } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return null;
        }
    }
}