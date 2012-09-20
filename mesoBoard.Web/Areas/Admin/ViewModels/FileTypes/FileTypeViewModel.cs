using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Data;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class FileTypeViewModel : IValidatableObject
    {
        public IEnumerable<FileType> FileTypes { get; set; }

        public int FileTypeID { get; set; }

        [Required]
        public string Extension { get; set; }

        public string Image { get; set; }

        public List<string> Images { get; set; }

        public SelectList ImagesList
        {
            get
            {
                return new SelectList(Images);
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Extension.StartsWith("."))
                yield return new ValidationResult("Include '.' in the file extension.", new[] { "Extension" });
        }
    }
}