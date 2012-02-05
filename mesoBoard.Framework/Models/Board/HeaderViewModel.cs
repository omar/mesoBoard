using mesoBoard.Data;
namespace mesoBoard.Framework.Models
{
    public class HeaderViewModel
    {
        public User CurrentUser { get; set; }
        public int NewMessagesCount { get; set; }
        public bool IsAdministrator { get; set; }
    }
}