using System.Collections.Generic;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Web.Areas.Admin.ViewModels;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public class SmiliesController : BaseAdminController
    {
        private IRepository<Smiley> _smileyRepository;

        public SmiliesController(IRepository<Smiley> smileyRepository)
        {
            _smileyRepository = smileyRepository;
            SetCrumb("Smilies");
        }

        [HttpPost]
        public ActionResult DeleteSmiley(int SmileyID)
        {
            _smileyRepository.Delete(SmileyID);
            SetSuccess("Smiley deleted");
            return RedirectToAction("Smilies");
        }

        [HttpGet]
        public ActionResult Smilies()
        {
            IEnumerable<Smiley> smilies = _smileyRepository.Get();
            SmiliesViewer model = new SmiliesViewer()
            {
                SmileyViewModel = new SmileyViewModel(),
                Smilies = smilies
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Smilies(SmileyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_smileyRepository.First(item => item.Code.ToLower() == model.Code.ToLower()) != null)
                    ModelState.AddModelError("Code", "A smiley with this code already exists.");
            }

            if (IsModelValidAndPersistErrors())
            {
                Smiley smiley = new Smiley()
                {
                    Code = model.Code,
                    ImageURL = model.ImageURL,
                    Title = model.Title
                };
                _smileyRepository.Add(smiley);
                SetSuccess("Smiley created");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult EditSmiley(int SmileyID)
        {
            Smiley smiley = _smileyRepository.Get(SmileyID);

            SmileyViewModel model = new SmileyViewModel()
            {
                Code = smiley.Code,
                ImageURL = smiley.ImageURL,
                SmileyID = smiley.SmileyID,
                Title = smiley.Title
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditSmiley(SmileyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_smileyRepository.First(item => item.Code.ToLower() == model.Code.ToLower()) != null)
                    ModelState.AddModelError("Code", "A smiley with this code already exists.");
            }

            if (IsModelValidAndPersistErrors())
            {
                Smiley smiley = new Smiley()
                {
                    Code = model.Code,
                    ImageURL = model.ImageURL,
                    SmileyID = model.SmileyID,
                    Title = model.Title
                };
                _smileyRepository.Update(smiley);
                SetSuccess("Smiley saved");
                return RedirectToAction("Smilies");
            }

            return RedirectToSelf();
        }
    }
}