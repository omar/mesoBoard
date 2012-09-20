using System.Collections.Generic;
using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class ViewThreadViewModel
    {
        public Thread Thread { get; set; }

        public IEnumerable<PostRow> Posts { get; set; }

        public bool IsSubscribed { get; set; }

        public bool HasVoted { get; set; }

        public bool CanCastVote { get; set; }

        public ThreadActions ThreadActions { get; set; }

        public Pagination Pagination { get; set; }

        public ThreadPoll ThreadPoll { get; set; }

        public User CurrentUser { get; set; }
    }
}