using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class ReportedPost
    {
        public int PostID { get; set; }
        public DateTime Date { get; set; }

        public virtual Post Post { get; set; }
    }
}
