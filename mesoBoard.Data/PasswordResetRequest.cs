using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class PasswordResetRequest
    {
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public string Token { get; set; }

        public virtual User User { get; set; }
    }
}
