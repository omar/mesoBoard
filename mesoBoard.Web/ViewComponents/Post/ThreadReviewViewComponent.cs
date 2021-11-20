using mesoBoard.Data;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.ViewComponents.Post
{
    public class ThreadReview : ViewComponent
    {
        private readonly ThreadServices _threadServices;
        
        public ThreadReview(ThreadServices threadServices)
        {
            _threadServices = threadServices;
        }

        public IViewComponentResult Invoke(int threadID)
        {
            Thread thread = _threadServices.GetThread(threadID);
            return View(thread);
        }
    }
}