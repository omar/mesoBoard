using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class ThreadViewStamp
    {
        public int ViewID { get; set; }
        public int UserID { get; set; }
        public int ThreadID { get; set; }
        public DateTime Date { get; set; }

        public virtual Thread Thread { get; set; }
        public virtual User User { get; set; }
    }
}
