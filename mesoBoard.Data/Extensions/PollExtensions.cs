using System;
using System.Linq;

namespace mesoBoard.Data
{
    public partial class Poll
    {
        public int TotalVotes
        {
            get
            {
                return this.PollOptions.Sum(x => x.PollVotes.Count);
            }
        }

        public string PollOptionsAsString()
        {
            string retVal = "";
            foreach (PollOption po in this.PollOptions)
            {
                retVal += po.Text + Environment.NewLine;
            }

            return retVal;
        }
    }
}