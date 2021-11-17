using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class PluginConfig
    {
        public int PluginConfigID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string PluginGroup { get; set; }
        public string Note { get; set; }
        public string Options { get; set; }
    }
}
