using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using mesoBoard.Framework.Core;
using mesoBoard.Web.Areas.Admin.ViewModels;

namespace mesoBoard.Web.Areas.Admin.Controllers
{
    public class ThemesController : BaseAdminController
    {
        private IRepository<Theme> _themeRepository;
        private ThemeServices _themeServices;

        public ThemesController(IRepository<Theme> themeRepository, ThemeServices themeServices)
        {
            _themeRepository = themeRepository;
            _themeServices = themeServices;
            SetCrumb("Themes");
        }

        [HttpGet]
        public ActionResult Themes()
        {
            ThemeViewer model = new ThemeViewer()
            {
                DefaultThemeID = SiteConfig.BoardTheme.ToInt(),
                Themes = _themeRepository.Get()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteTheme(int ThemeID)
        {
            if (SiteConfig.BoardTheme.Value.Equals(ThemeID.ToString()))
            {
                SetError("The default theme can't be deleted until a new default theme is selected");
            }
            else
            {
                _themeRepository.Delete(ThemeID);
                SetSuccess("Theme deleted");
            }
            return RedirectToAction("Themes");
        }

        public ActionResult SetThemeDefault(int ThemeID)
        {
            _themeServices.ChangeDefaultTheme(ThemeID);
            SetSuccess("Theme default changed");
            return RedirectToAction("Themes");
        }

        [HttpGet]
        public ActionResult CreateTheme()
        {
            ThemeViewModel model = new ThemeViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateTheme(ThemeViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                Theme theme = new Theme()
                {
                    DisplayName = model.DisplayName,
                    FolderName = model.FolderName,
                    Name = model.Name,
                    VisibleToUsers = model.VisibleToUsers
                };
                _themeRepository.Add(theme);

                if (model.DefaultTheme)
                    _themeServices.ChangeDefaultTheme(theme.ThemeID);

                SetSuccess("Theme Created");
                return RedirectToAction("Themes");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        public ActionResult EditTheme(int ThemeID)
        {
            Theme theme = _themeRepository.Get(ThemeID);

            ThemeViewModel model = new ThemeViewModel()
            {
                DefaultTheme = SiteConfig.BoardTheme.ToInt() == theme.ThemeID,
                DisplayName = theme.DisplayName,
                FolderName = theme.FolderName,
                Name = theme.Name,
                ThemeID = theme.ThemeID,
                VisibleToUsers = theme.VisibleToUsers
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditTheme(ThemeViewModel model)
        {
            if(IsModelValidAndPersistErrors())
            {
                Theme theme = _themeRepository.Get(model.ThemeID);
                theme.DisplayName = model.DisplayName;
                theme.FolderName = model.FolderName;
                theme.Name = model.Name;
                theme.ThemeID = model.ThemeID;
                theme.VisibleToUsers = model.VisibleToUsers;
                _themeRepository.Update(theme);

                if (model.DefaultTheme)
                    _themeServices.ChangeDefaultTheme(theme.ThemeID);

                SetSuccess("Theme updated");
                return RedirectToAction("Themes");
            }

            return RedirectToSelf(new { ThemeID = model.ThemeID });
        }
    }
}
