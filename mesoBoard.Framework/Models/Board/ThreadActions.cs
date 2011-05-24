using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class ThreadActions : BaseViewModel
    {
        public Thread Thread { get; set; }
        public bool CanLock { get; set; }
    }
}
