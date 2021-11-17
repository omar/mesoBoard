using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class OnlineGuest
    {
        public int OnlineGuestID { get; set; }
        public string IP { get; set; }
        public DateTime Date { get; set; }
    }
}
