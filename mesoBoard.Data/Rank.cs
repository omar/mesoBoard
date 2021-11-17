using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Rank
    {
        public Rank()
        {
            Roles = new HashSet<Role>();
        }

        public int RankID { get; set; }
        public string Title { get; set; }
        public int PostCount { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public bool IsRoleRank { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
