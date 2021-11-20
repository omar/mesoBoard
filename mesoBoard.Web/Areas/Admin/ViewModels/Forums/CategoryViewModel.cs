using System.ComponentModel.DataAnnotations;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}