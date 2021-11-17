using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class UserProfile
    {
        public int UserID { get; set; }
        public bool AlwaysSubscribeToThread { get; set; }
        public bool AlwaysShowSignature { get; set; }
        public int? ThemeID { get; set; }
        public string AvatarType { get; set; }
        public string Avatar { get; set; }
        public string Location { get; set; }
        public int? DefaultRole { get; set; }
        public string AIM { get; set; }
        public int? ICQ { get; set; }
        public string MSN { get; set; }
        public string Website { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Signature { get; set; }
        public string ParsedSignature { get; set; }

        public virtual Role Role { get; set; }
        public virtual Theme Theme { get; set; }
        public virtual User User { get; set; }
    }
}
