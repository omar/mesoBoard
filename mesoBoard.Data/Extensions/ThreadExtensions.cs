using System.Linq;
using mesoBoard.Common;

namespace mesoBoard.Data
{
    public partial class Thread
    {
        public bool IsSticky
        {
            get
            {
                return this.IsType(ThreadType.Sticky);
            }
        }

        public bool IsAnnouncement
        {
            get
            {
                return this.IsType(ThreadType.Announcement);
            }
        }

        public bool IsGlobalAnnouncement
        {
            get
            {
                return this.IsType(ThreadType.GlobalAnnouncement);
            }
        }

        public bool IsType(ThreadType threadType)
        {
            return this.Type == (int)threadType;
        }

        public Post FirstPost
        {
            get
            {
                var firstPost = this.Posts.OrderBy(item => item.Date).First();
                return firstPost;
            }
        }

        public Post LastPost
        {
            get
            {
                var lastPost = this.Posts.OrderByDescending(item => item.Date).First();
                return lastPost;
            }
        }

        public int Views
        {
            get
            {
                return this.ThreadViews.Count;
            }
        }
    }
}