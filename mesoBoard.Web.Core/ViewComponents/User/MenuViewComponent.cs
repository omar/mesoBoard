using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.ViewComponents.Post
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly User _currentUser;
        private readonly MessageServices _messageServices;
        
        public MenuViewComponent(MessageServices messageServices, User currentUser)
        {
            _currentUser = currentUser;
            _messageServices = messageServices;
        }

        public IViewComponentResult Invoke(int x, int z)
        {
            int messageCount = HttpContext.User.Identity.IsAuthenticated ? _messageServices.GetUnreadMessages(_currentUser.UserID).Count() : 0;
            ViewData["NewMessagesCount"] = messageCount;
            return View();
        }
    }
}