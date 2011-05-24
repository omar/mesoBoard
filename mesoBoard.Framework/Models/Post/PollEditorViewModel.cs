using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace mesoBoard.Framework.Models
{
    public class PollEditorViewModel : IValidatableObject
    {
        public int PollID { get; set; }
        public string Text { get; set; }
        public string Options { get; set; }
        public bool Delete { get; set; }

        public bool HasVotes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var OptionsError = new ValidationResult("The poll must have at least two options.", new[] { "Options" });

            if (string.IsNullOrWhiteSpace(Text) && !string.IsNullOrWhiteSpace(Options))
                yield return new ValidationResult("Enter a poll question.", new[] { "Text" });
            else if (!string.IsNullOrWhiteSpace(Text) && string.IsNullOrWhiteSpace(Options))
                yield return OptionsError;

            if (!string.IsNullOrWhiteSpace(Options))
            {
                var splitOptions = Options.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (splitOptions.Count() < 2)
                    yield return OptionsError;
            }
        }

        public string[] OptionsSplit
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Options))
                    return new string[] { };

                return Options.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}