using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class ThreadActions 
    {
        public Thread Thread { get; set; }
        public bool CanLock { get; set; }
        public User CurrentUser { get; set; }
    }
}
