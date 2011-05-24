using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class UserProfileViewModel : BaseViewModel
    {
        public Post LastPost { get; set; }
        public User User { get; set; }
        public UserProfile Profile { get; set; }
    }
}