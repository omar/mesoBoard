using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using mesoBoard.Framework.Core;
using System.Collections.Generic;
using System.Linq;
using mesoBoard.Framework.Models;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        private IRepository<User> _userRepository;
        private UserServices _userServices;

        public UsersController(IRepository<User> userRepository, UserServices userServices)
        {
            _userRepository = userRepository;
            _userServices = userServices;
            SetCrumb("Users");
        }

        public ActionResult ChangeUserStatus(int userID)
        {
            User user = _userRepository.Get(userID);
            if (string.IsNullOrEmpty(user.ActivationCode))
            {
                user.ActivationCode = Randoms.CleanGUID();
                SetSuccess("User deactivated");
            }
            else
            {
                user.ActivationCode = string.Empty;
                SetSuccess("User activated");
            }
            _userRepository.Update(user);
            return RedirectToAction("UserDetails", new { UserID = userID });
        }

        public ActionResult Users(int page = 1, int pageSize = 25, string sort = "Username")
        {
            IEnumerable<User> users = _userRepository.Get();
            switch (sort)
            {
                case "Status":
                    users = users.OrderBy(item => item.ActivationCode);
                    break;
                case "Email":
                    users = users.OrderBy(item => item.Email);
                    break;
                case "Username":
                default:
                    users = users.OrderBy(item => item.Username);
                    break;
            }
            int count = users.Count();
            users = users.TakePage(page, pageSize);
            Pagination pagination = new Pagination(page, count, pageSize, new { area = "Admin", action = "Users", controller = "Users", sort = sort });
            ViewData["Pagination"] = pagination;
            return View(users);
        }

        public ActionResult UserDetails(int userID)
        {
            User user = _userRepository.Get(userID);
            return View(user);
        }

        [HttpPost]
        public ActionResult DeleteUser(int userID)
        {
            _userServices.DeleteUser(userID);
            SetSuccess("User deleted");
            return RedirectToAction("Users");
        }
    }
}
