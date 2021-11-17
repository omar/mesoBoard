using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Theme
    {
        public Theme()
        {
            UserProfiles = new HashSet<UserProfile>();
        }

        public int ThemeID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool VisibleToUsers { get; set; }
        public string FolderName { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
