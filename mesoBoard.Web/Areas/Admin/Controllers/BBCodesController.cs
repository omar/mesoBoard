using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Web.Areas.Admin.ViewModels;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public class BBCodesController : BaseAdminController
    {
        private IRepository<BBCode> _bbCodeRepository;

        public BBCodesController(IRepository<BBCode> bbCodeRepository)
        {
            _bbCodeRepository = bbCodeRepository;
            SetCrumb("BB Codes");
        }

        [HttpGet]
        public ActionResult BBCodes()
        {
            var bbCodes = _bbCodeRepository.Get();
            BBCodeViewModel model = new BBCodeViewModel()
            {
                BBCodes = bbCodes
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult BBCodes(BBCodeViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                BBCode bbCode = new BBCode()
                {
                    Parse = model.Parse,
                    Tag = model.Tag
                };

                _bbCodeRepository.Add(bbCode);
                SetSuccess("BB Code created");
            }

            return RedirectToSelf();
        }

        [HttpPost]
        public ActionResult DeleteBBCode(int BBCodeID)
        {
            _bbCodeRepository.Delete(BBCodeID);
            SetSuccess("BB Code deleted");
            return RedirectToAction("BBCodes");
        }

        [HttpGet]
        public ActionResult EditBBCode(int BBCodeID)
        {
            BBCode bbCode = _bbCodeRepository.Get(BBCodeID);
            BBCodeViewModel model = new BBCodeViewModel()
            {
                Tag = bbCode.Tag,
                Parse = bbCode.Parse,
                BBCodeID = bbCode.BBCodeID
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditBBCode(BBCodeViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                BBCode bbCode = _bbCodeRepository.Get(model.BBCodeID);
                TryUpdateModel(bbCode);
                _bbCodeRepository.Update(bbCode);
                SetSuccess("BB Code edited");
                return RedirectToAction("BBCodes");
            }

            return RedirectToSelf(new { BBCodeID = model.BBCodeID });
        }
    }
}