using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Thread
    {
        public Thread()
        {
            Posts = new HashSet<Post>();
            Subscriptions = new HashSet<Subscription>();
            ThreadViewStamps = new HashSet<ThreadViewStamp>();
            ThreadViews = new HashSet<ThreadView>();
        }

        public int ThreadID { get; set; }
        public int ForumID { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public bool HasPoll { get; set; }
        public bool IsLocked { get; set; }
        public string Image { get; set; }

        public virtual Forum Forum { get; set; }
        public virtual Poll Poll { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<ThreadViewStamp> ThreadViewStamps { get; set; }
        public virtual ICollection<ThreadView> ThreadViews { get; set; }
    }
}
