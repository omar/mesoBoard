using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesoBoard.Data;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class SmiliesViewer
    {
        public IEnumerable<Smiley> Smilies { get; set; }

        public SmileyViewModel SmileyViewModel { get; set; }
    }
}