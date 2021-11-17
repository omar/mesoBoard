using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class OnlineUser
    {
        public int UserID { get; set; }
        public DateTime Date { get; set; }

        public virtual User User { get; set; }
    }
}
