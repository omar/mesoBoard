using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class User
    {
        public User()
        {
            InRoles = new HashSet<InRole>();
            MessageFromUsers = new HashSet<Message>();
            MessageToUsers = new HashSet<Message>();
            PollVotes = new HashSet<PollVote>();
            Posts = new HashSet<Post>();
            Subscriptions = new HashSet<Subscription>();
            ThreadViewStamps = new HashSet<ThreadViewStamp>();
            ThreadViews = new HashSet<ThreadView>();
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string ActivationCode { get; set; }
        public bool Status { get; set; }
        public DateTime RegisterDate { get; set; }
        public string RegisterIP { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }
        public DateTime LastLogoutDate { get; set; }
        public DateTime? LastPostDate { get; set; }
        public string UsernameLower { get; set; }

        public virtual OnlineUser OnlineUser { get; set; }
        public virtual PasswordResetRequest PasswordResetRequest { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<InRole> InRoles { get; set; }
        public virtual ICollection<Message> MessageFromUsers { get; set; }
        public virtual ICollection<Message> MessageToUsers { get; set; }
        public virtual ICollection<PollVote> PollVotes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<ThreadViewStamp> ThreadViewStamps { get; set; }
        public virtual ICollection<ThreadView> ThreadViews { get; set; }
    }
}
