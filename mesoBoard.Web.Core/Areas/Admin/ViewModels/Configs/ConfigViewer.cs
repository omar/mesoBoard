using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class ConfigViewer
    {
        public string[] ConfigGroups { get; set; }

        public IEnumerable<ConfigViewModel> Configs { get; set; }

        public SelectList ThemeList { get; set; }

        public SelectList RolesList { get; set; }

        public string BoardTheme { get; set; }
    }
}