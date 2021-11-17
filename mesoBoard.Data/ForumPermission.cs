using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class ForumPermission
    {
        public int ForumPermissionID { get; set; }
        public int ForumID { get; set; }
        public int Visibility { get; set; }
        public int Posting { get; set; }
        public int Polling { get; set; }
        public int Attachments { get; set; }
        public int RoleID { get; set; }

        public virtual Forum Forum { get; set; }
        public virtual Role Role { get; set; }
    }
}
