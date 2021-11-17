using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class ThemeServices : BaseService
    {
        private IRepository<Theme> _themeRepository;
        private IRepository<Config> _configRepository;
        private SiteConfig _siteConfig;

        public ThemeServices(
            IRepository<Theme> themeRepository,
            IRepository<Config> configRepository,
            SiteConfig siteConfig,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _themeRepository = themeRepository;
            _configRepository = configRepository;
            _siteConfig = siteConfig;
        }

        public Theme GetAdminTheme()
        {
            return _themeRepository.First(item => item.FolderName == "Default");
        }

        public Theme GetDefaultTheme()
        {
            var theme = _themeRepository.Get(int.Parse(SiteConfig.BoardTheme.Value));
            return theme;
        }

        public Theme GetTheme(User currentUser, string controllerName, string previewTheme)
        {
            Theme theme;

            if (!string.IsNullOrWhiteSpace(previewTheme))
                theme = _themeRepository.Get(int.Parse(previewTheme));
            else if (bool.Parse(SiteConfig.OverrideUserTheme.Value) || currentUser == null || currentUser.UserID == 0 || !currentUser.UserProfile.ThemeID.HasValue)
                theme = _themeRepository.Get(int.Parse(SiteConfig.BoardTheme.Value));
            else
                theme = _themeRepository.Get(currentUser.UserProfile.ThemeID.Value);

            return theme;
        }

        public IEnumerable<Theme> GetVisibleThemes()
        {
            return _themeRepository.Where(y => y.VisibleToUsers == true).ToList();
        }

        public void ChangeDefaultTheme(int themeID)
        {
            Config boardTheme = _configRepository.Get(SiteConfig.BoardTheme.ConfigID);
            boardTheme.Value = themeID.ToString();
            _configRepository.Update(boardTheme);
            _unitOfWork.Commit();
            _siteConfig.UpdateCache();
        }
    }
}