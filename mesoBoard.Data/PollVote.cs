using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class PollVote
    {
        public int PollVoteID { get; set; }
        public int PollOptionID { get; set; }
        public int UserID { get; set; }

        public virtual PollOption PollOption { get; set; }
        public virtual User User { get; set; }
    }
}
