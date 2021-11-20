using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mesoBoard.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class ForumViewModel
    {
        public int ForumID { get; set; }

        [Required(ErrorMessage = "Select a category.")]
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Visible to Guests")]
        public bool VisibleToGuests { get; set; }

        [Required]
        [Display(Name = "Allow guests to download attachments")]
        public bool AllowGuestDownloads { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public SelectList CategoryList
        {
            get
            {
                return new SelectList(Categories, "CategoryID", "Name");
            }
        }
    }
}