using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Config
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
