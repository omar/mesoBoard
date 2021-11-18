using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.ViewComponents.Post
{
    public class ThreadReviewViewComponent : ViewComponent
    {
        private readonly ThreadServices _threadServices;
        
        public ThreadReviewViewComponent(ThreadServices threadServices)
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