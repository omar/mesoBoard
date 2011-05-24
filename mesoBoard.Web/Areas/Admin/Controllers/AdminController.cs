using System.Collections.Generic;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public partial class AdminController : BaseAdminController
    {
        IRepository<User> _userRepository;
        IRepository<ReportedPost> _reportedPostRepository;

        public AdminController(IRepository<User> userRepository, IRepository<ReportedPost> reportedPostRepository)
        {
            _userRepository = userRepository;
            _reportedPostRepository = reportedPostRepository;
        }

        public ActionResult Index()
        {
            ViewData["NewestRegistrations"] = _userRepository.Get();
            ViewData["ReportedPosts"] = _reportedPostRepository.Get();
            SetCrumb("Summary");
            return View();
        }

        public ActionResult ReportedPosts()
        {
            SetCrumb("Reported Posts");
            IEnumerable<ReportedPost> reported = _reportedPostRepository.Get();
            return View(reported);
        }

        public ActionResult MarkAsSafe(int ReportedPostID)
        {
            _reportedPostRepository.Delete(ReportedPostID);
            SetSuccess("Post marked as safe");
            return RedirectToAction("ReportedPosts");
        }

        public ActionResult Confirm(string YesRedirect, string NoRedirect)
        {
            ViewData["YesUrl"] = YesRedirect;
            ViewData["NoUrl"] = NoRedirect;
            return View();
        }
    }
}