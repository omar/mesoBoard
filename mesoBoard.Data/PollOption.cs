using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class PollOption
    {
        public PollOption()
        {
            PollVotes = new HashSet<PollVote>();
        }

        public int PollOptionID { get; set; }
        public string Text { get; set; }
        public int PollID { get; set; }

        public virtual Poll Poll { get; set; }
        public virtual ICollection<PollVote> PollVotes { get; set; }
    }
}
