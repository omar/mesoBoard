using System.Collections.Generic;
using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class BoardStatsViewModel 
    {
        public IEnumerable<OnlineUserDetails> OnlineUsers { get; set; }
        public IEnumerable<OnlineGuest> OnlineGuests { get; set; }
        public int TotalRegisteredUsers { get; set; }
        public int TotalThreads { get; set; }
        public int TotalPosts { get; set; }
        public User NewestUser { get; set; }
        public IEnumerable<User> BirthdayUsers { get; set; }
    }
}