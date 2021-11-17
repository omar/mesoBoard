using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Forum
    {
        public Forum()
        {
            ForumPermissions = new HashSet<ForumPermission>();
            Threads = new HashSet<Thread>();
        }

        public int ForumID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool AllowGuestDownloads { get; set; }
        public bool VisibleToGuests { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ForumPermission> ForumPermissions { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
    }
}
