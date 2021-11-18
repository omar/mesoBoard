using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.ViewComponents.Post
{
    public class GetSmiliesViewComponent : ViewComponent
    {
        private readonly IRepository<Smiley> _smileyRepository;
        
        public GetSmiliesViewComponent(IRepository<Smiley> smileyRepository)
        {
            _smileyRepository = smileyRepository;
        }

        public IViewComponentResult Invoke(int x, int z)
        {
            ViewData["x"] = x;
            ViewData["z"] = z;
            IEnumerable<Smiley> smilies = _smileyRepository.Get().ToList();
            if (smilies.Count() < z)
            {
                ViewData["z"] = smilies.Count();
                return View(smilies);
            }
            else
            {
                return View(smilies.Take(z));
            }
        }
    }
}