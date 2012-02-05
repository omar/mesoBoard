using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class ThreadPoll 
    {
        public Poll Poll;
        public bool CanCastVote;
        public User CurrentUser { get; set; }
    }
}
