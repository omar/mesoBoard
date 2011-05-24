using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class PostRow : BaseViewModel
    {
        public Post Post { get; set; }
        public bool IsOdd { get; set; }
        public Thread Thread { get; set; }
        public bool CanPost { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool IsLastPost { get; set; }
    }
}
