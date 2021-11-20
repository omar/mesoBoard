using System.Linq;
using mesoBoard.Data;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.ViewComponents.Post
{
    public class Menu : ViewComponent
    {
        private readonly User _currentUser;
        private readonly MessageServices _messageServices;
        
        public Menu(MessageServices messageServices, User currentUser)
        {
            _currentUser = currentUser;
            _messageServices = messageServices;
        }

        public IViewComponentResult Invoke()
        {
            int messageCount = HttpContext.User.Identity.IsAuthenticated ? _messageServices.GetUnreadMessages(_currentUser.UserID).Count() : 0;
            ViewData["NewMessagesCount"] = messageCount;
            return View();
        }
    }
}