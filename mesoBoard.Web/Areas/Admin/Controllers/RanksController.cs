using System.Collections.Generic;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public class RanksController : BaseAdminController
    {
        private IRepository<Rank> _rankRepository;

        public RanksController(IRepository<Rank> rankRepository)
        {
            _rankRepository = rankRepository;
            SetCrumb("Ranks");
        }

        public ActionResult Ranks()
        {
            IEnumerable<Rank> Ranks = _rankRepository.Get();
            return View(Ranks);
        }

        [HttpPost]
        public ActionResult DeleteRank(int RankID)
        {
            _rankRepository.Delete(RankID);
            SetSuccess("Rank deleted");
            return RedirectToAction("Ranks");
        }

        [HttpGet]
        public ActionResult CreateRank()
        {
            RankViewModel model = new RankViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateRank(RankViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.IsRoleRank && _rankRepository.First(x => x.PostCount == model.PostCount) != null)
                    ModelState.AddModelError("PostCount", "A rank with " + model.PostCount + " post count already exists");
            }

            if (IsModelValidAndPersistErrors())
            {
                Rank rank = new Rank()
                {
                    Color = model.Color,
                    Image = model.Image,
                    IsRoleRank = model.IsRoleRank,
                    PostCount = model.PostCount,
                    Title = model.Title
                };
                _rankRepository.Add(rank);
                SetSuccess("Rank created");
                return RedirectToAction("Ranks");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult EditRank(int RankID)
        {
            Rank rank = _rankRepository.Get(RankID);
            RankViewModel model = new RankViewModel()
            {
                RankID = rank.RankID,
                Color = rank.Color,
                Image = rank.Image,
                IsRoleRank = rank.IsRoleRank,
                PostCount = rank.PostCount,
                Title = rank.Title
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditRank(RankViewModel model)
        {
            if (ModelState.IsValid)
            {
                Rank rankExists = _rankRepository.First(item => item.RankID != model.RankID && item.PostCount.Equals(model.PostCount));
                if (rankExists != null && !model.IsRoleRank)
                    ModelState.AddModelError("PostCount", "A rank with " + model.PostCount + " post count already exists");
            }

            if (IsModelValidAndPersistErrors())
            {
                Rank rank = _rankRepository.Get(model.RankID);
                rank.RankID = model.RankID;
                rank.Color = model.Color;
                rank.Image = model.Image;
                rank.IsRoleRank = model.IsRoleRank;
                rank.PostCount = model.PostCount;
                rank.Title = model.Title;
                _rankRepository.Update(rank);
                SetSuccess("Rank updated");
                return RedirectToAction("Ranks");
            }

            return RedirectToSelf(new { RankID = model.RankID });
        }
    }
}