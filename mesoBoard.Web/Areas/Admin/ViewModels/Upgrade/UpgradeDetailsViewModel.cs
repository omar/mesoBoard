using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class UpgradeDetailsViewModel
    {
        public string Version { get; set; }
        public bool HasDetails { get; set; }
        public string Details { get; set; }
    }
}