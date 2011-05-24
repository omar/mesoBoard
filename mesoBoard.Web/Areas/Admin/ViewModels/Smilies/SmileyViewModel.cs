using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class SmileyViewModel
    {
        public int SmileyID { get; set; }

        [Required]
        public string Code { get; set; }

        public string Title { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        public string ImageURL { get; set; }

        public string ImagePath
        {
            get
            {
                return Path.Combine(DirectoryPaths.Smilies, ImageURL);
            }
        }
    }
}