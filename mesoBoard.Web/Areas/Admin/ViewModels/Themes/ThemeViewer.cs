using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesoBoard.Data;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class ThemeViewer
    {
        public IEnumerable<Theme> Themes { get; set; }

        public int DefaultThemeID { get; set; }
    }
}