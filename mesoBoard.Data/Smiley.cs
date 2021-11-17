using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Smiley
    {
        public int SmileyID { get; set; }
        public string ImageURL { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
    }
}
