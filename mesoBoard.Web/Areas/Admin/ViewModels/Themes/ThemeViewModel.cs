using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class ThemeViewModel
    {
        public int ThemeID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Folder Name")]
        public string FolderName { get; set; }

        [Display(Name = "Visible to Users")]
        public bool VisibleToUsers { get; set; }

        [Display(Name = "Default Theme")]
        public bool DefaultTheme { get; set; }
    }
}