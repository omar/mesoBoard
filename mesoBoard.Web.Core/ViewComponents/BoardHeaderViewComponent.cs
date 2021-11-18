using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Models;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.ViewComponents
{
    public class BoardHeaderViewComponent : ViewComponent
    {
        private readonly MessageServices _messageServices;
        private readonly RoleServices _roleServices;
        private readonly User _currentUser;

        public BoardHeaderViewComponent(
            MessageServices messageServices,
            RoleServices roleServices,
            User currentUser
        )
        {
            _messageServices = messageServices;
            _roleServices = roleServices;
            _currentUser = currentUser;
        }

        public IViewComponentResult Invoke()
        {
            var model = new HeaderViewModel()
            {
                CurrentUser = _currentUser,
                NewMessagesCount = HttpContext.User.Identity.IsAuthenticated ? _messageServices.GetUnreadMessages(_currentUser.UserID).Count() : 0,
                IsAdministrator = HttpContext.User.Identity.IsAuthenticated ? _roleServices.UserHasSpecialPermissions(_currentUser.UserID, SpecialPermissionValue.Administrator) : false
            };

            return View(model);
        }
    }
}