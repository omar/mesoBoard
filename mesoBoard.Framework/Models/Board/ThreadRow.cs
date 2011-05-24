using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class ThreadRow : BaseViewModel
    {
        public Thread Thread { get; set; }
        public Post FirstPost { get; set; }
        public Post LastPost { get; set; }
        public int TotalPosts { get; set; }
        public bool IsOdd { get; set; }
        public bool HasNewPost  { get; set; }
        public bool IsSubscribed { get; set; }
        public bool HasAttachment { get; set; }
    }
}
