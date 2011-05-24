using System.Collections.Generic;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class ThemeServices 
    {
        IRepository<Theme> _themeRepository;
        IRepository<Config> _configRepository;

        public ThemeServices(IRepository<Theme> themeRepository, IRepository<Config> configRepository)
        {
            this._themeRepository = themeRepository;
            this._configRepository = configRepository;
        }

        public Theme GetAdminTheme()
        {
            return _themeRepository.First(item => item.FolderName == "Default");
        }

        public Theme GetTheme(User currentUser, string controllerName, string previewTheme)
        {
            Theme theme;

            if (previewTheme != null)
                theme = _themeRepository.Get(int.Parse(previewTheme));
            else if (bool.Parse(SiteConfig.OverrideUserTheme.Value) || currentUser == null || currentUser.UserID == 0 || !currentUser.UserProfile.ThemeID.HasValue)
                theme = _themeRepository.Get(int.Parse(SiteConfig.BoardTheme.Value));
            else
                theme = _themeRepository.Get(currentUser.UserProfile.ThemeID.Value);

            return theme;
        }

        public IEnumerable<Theme> GetVisibleThemes()
        {
            return _themeRepository.Where(y => y.VisibleToUsers == true);
        }

        public void ChangeDefaultTheme(int themeID)
        {
            Config boardTheme = _configRepository.Get(SiteConfig.BoardTheme.ConfigID);
            boardTheme.Value = themeID.ToString();
            _configRepository.Update(boardTheme);
            SiteConfig.UpdateCache();
        }
    }
}