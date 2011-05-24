using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class ForumRow : BaseViewModel
    {
        public Forum Forum { get; set; }
        public bool IsOdd { get; set; }
        public bool HasNewPost { get; set; }
        public Post LastPost { get; set; }
    }
}
