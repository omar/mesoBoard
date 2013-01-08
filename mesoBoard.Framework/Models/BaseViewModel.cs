using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class BaseViewModel
    {
        public User CurrentUser { get; set; }

        public bool IsAuthenticated { get; set; }

        public Theme CurrentTheme { get; set; }
    }
}