using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }
    }
}