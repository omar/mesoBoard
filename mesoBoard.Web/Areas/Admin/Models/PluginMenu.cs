using System.Collections.Generic;
using mesoBoard.Common;

namespace mesoBoard.Web.Areas.Admin.Models
{
    public class PluginMenu
    {
        public NavigationLink MainLink { get; set; }

        public IList<NavigationLink> ChildLinks { get; set; }
    }
}