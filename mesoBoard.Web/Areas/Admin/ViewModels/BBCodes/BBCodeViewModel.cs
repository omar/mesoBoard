using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesoBoard.Data;
using System.ComponentModel.DataAnnotations;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class BBCodeViewModel
    {
        public IEnumerable<BBCode> BBCodes { get; set; }

        [Required]
        public string Tag { get; set; }

        [Required]
        public string Parse { get; set; }

        public int BBCodeID { get; set; }
    }
}