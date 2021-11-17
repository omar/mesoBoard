using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Role
    {
        public Role()
        {
            ForumPermissions = new HashSet<ForumPermission>();
            InRoles = new HashSet<InRole>();
            UserProfiles = new HashSet<UserProfile>();
        }

        public int RoleID { get; set; }
        public string Name { get; set; }
        public int? RankID { get; set; }
        public bool IsGroup { get; set; }
        public byte SpecialPermissions { get; set; }

        public virtual Rank Rank { get; set; }
        public virtual ICollection<ForumPermission> ForumPermissions { get; set; }
        public virtual ICollection<InRole> InRoles { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
