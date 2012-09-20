using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class ConfigViewModel
    {
        public int ConfigID { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public string Group { get; set; }

        public string Note { get; set; }

        public string Options { get; set; }
    }
}