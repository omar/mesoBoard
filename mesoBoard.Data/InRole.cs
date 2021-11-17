using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class InRole
    {
        public int InRoleID { get; set; }
        public int RoleID { get; set; }
        public int UserID { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
