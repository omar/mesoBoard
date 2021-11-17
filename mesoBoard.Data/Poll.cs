using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Poll
    {
        public Poll()
        {
            PollOptions = new HashSet<PollOption>();
        }

        public int PollID { get; set; }
        public string Question { get; set; }

        public virtual Thread PollNavigation { get; set; }
        public virtual ICollection<PollOption> PollOptions { get; set; }
    }
}
