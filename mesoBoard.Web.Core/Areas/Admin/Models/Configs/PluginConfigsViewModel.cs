using System.Collections.Generic;
using mesoBoard.Data;

namespace mesoBoard.Web.Areas.Admin.Models
{
    public class PluginConfigsViewModel
    {
        public IEnumerable<PluginConfig> Configs { get; set; }

        public string[] ConfigGroups { get; set; }
    }
}