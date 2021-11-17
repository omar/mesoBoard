using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Framework.Models;
using mesoBoard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.Controllers
{
    public class MembersController : BaseController
    {
        private IRepository<Role> _roleRepository;
        private IRepository<User> _userRepository;
        private IRepository<Thread> _threadRepository;
        private IRepository<Post> _postRepository;
        private UserServices _userServices;

        public MembersController(
             IRepository<Role> roleRepository,
             IRepository<User> userRepository,
             IRepository<Thread> threadRepository,
             IRepository<Post> postRepository,
             UserServices userServices)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _threadRepository = threadRepository;
            _postRepository = postRepository;
            _userServices = userServices;
            SetTopBreadCrumb("Members");
        }

        public ActionResult ViewGroup(int GroupID, int PageSize = 10, int Page = 1)
        {
            Role group = _roleRepository.Get(GroupID);

            if (!group.IsGroup)
            {
                SetError("Invalid group ID");
                return RedirectToAction("Groups");
            }

            IEnumerable<User> usersInRole = group.InRoles.Select(x => x.User);

            int MemberCount = usersInRole.Count();

            usersInRole = usersInRole.TakePage(Page, PageSize);

            ViewData["Pagination"] = new Pagination(Page, MemberCount, PageSize, "ViewGroup", "Members", new { GroupID = GroupID });

            ViewData["Users"] = usersInRole;

            return View(group);
        }

        public ActionResult Groups(int Page = 1, int PageSize = 10)
        {
            SetBreadCrumb("Groups");
            IEnumerable<Role> groups = _roleRepository.Where(x => x.IsGroup == true).ToList();

            int MemberCount = groups.Count();

            groups = groups.TakePage(Page, PageSize);

            ViewData["Pagination"] = new Pagination(Page, MemberCount, PageSize, "Groups", "Members");

            return View(groups);
        }

        [Route("")]
        public ActionResult MembersList(string Letter = "(All)", int Page = 1, int PageSize = 10)
        {
            SetBreadCrumb("Members List");
            IEnumerable<User> Members = _userRepository.Get();
            ViewData["letter"] = Letter;
            if (Letter != "(All)")
                Members = Members.Where(y => y.UsernameLower[0].ToString() == Letter.ToLower());

            int MemberCount = Members.Count();

            Members = Members.TakePage(Page, PageSize);

            ViewData["Pagination"] = new Pagination(Page, MemberCount, PageSize, "MembersList", "Members", new { Letter = Letter });

            return View(Members.ToList());
        }

        public ActionResult UserProfile(string UserNameOrID)
        {
            int tryInt;
            User user;

            if (int.TryParse(UserNameOrID, out tryInt))
                user = _userServices.GetUser(tryInt);
            else
                user = _userServices.GetUser(UserNameOrID);

            if (user != null)
            {
                int totalThreadsCreated = _threadRepository.Get().Select(item => item.FirstPost.UserID == user.UserID).Count();

                var model = new UserProfileViewModel()
                {
                    LastPost = _postRepository.Where(item => item.UserID.Equals(user.UserID)).OrderByDescending(item => item.Date).FirstOrDefault(),
                    Profile = user.UserProfile,
                    User = user
                };

                SetBreadCrumb(user.Username + "'s Profile");
                return View(model);
            }
            else
            {
                SetError("No such user found");
                return RedirectToAction("MembersList");
            }
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult ChangeTheme(int PreviewTheme, string Action)
        {
            if (Action == "Preview")
            {
                HttpContext.Session.SetInt32("ptheme", PreviewTheme);
                SetSuccess("Theme changed");
            }
            else if (Action == "Reset")
            {
                if (HttpContext.Session.GetInt32("ptheme") != null)
                {
                    HttpContext.Session.Remove("ptheme");
                }
            }

            return Redirect(Request.GetTypedHeaders().Referer.AbsolutePath);
        }
    }
}