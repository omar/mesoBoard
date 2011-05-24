using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class PostActions : BaseViewModel
    {
        public Post ThePost { get; set; }
        public bool CanPost { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
