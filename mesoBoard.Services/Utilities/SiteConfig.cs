using System.Collections.Generic;
using System.Linq;
using mesoBoard.Data;
using mesoBoard.Data.Repositories;
using System.Runtime.Caching;
using mesoBoard.Common;

namespace mesoBoard.Services
{
    public class SiteConfig
    {
        private readonly IRepository<Config> _configRepository;

        public SiteConfig(IRepository<Config> configRepository)
        {
            _configRepository = configRepository;
        }

        // Board Settings
        public static Config BoardName              { get { return GetConfig("BoardName"); } }
        public static Config BoardURL               { get { return GetConfig("BoardURL"); } }
        public static Config BoardCreateDate        { get { return GetConfig("BoardCreateDate"); } }
        public static Config BoardTheme             { get { return GetConfig("BoardTheme"); } }
        public static Config PageTitlePhrase        { get { return GetConfig("PageTitlePhrase"); } }
        public static Config OverrideUserTheme      { get { return GetConfig("OverrideUserTheme"); } }
        public static Config Language               { get { return GetConfig("Language"); } }
        public static Config TimeOffset             { get { return GetConfig("TimeOffset"); } }
        public static Config PostsPerPage           { get { return GetConfig("PostsPerPage"); } }
        public static Config ThreadsPerPage         { get { return GetConfig("ThreadsPerPage"); } }
        public static Config AutomatedFromEmail     { get { return GetConfig("AutomatedFromEmail"); } }
        public static Config Offline                { get { return GetConfig("Offline"); } }

        // Avatar
        public static Config AvatarWidth            { get { return GetConfig("AvatarWidth"); } }
        public static Config AvatarHeight           { get { return GetConfig("AvatarHeight"); } }

        // Posting
        public static Config TimeBetweenPosts       { get { return GetConfig("TimeBetweenPosts"); } }
        public static Config AllowThreadImage       { get { return GetConfig("AllowThreadImage"); } }
        public static Config SyntaxHighlighting     { get { return GetConfig("SyntaxHighlighting"); } }
        public static Config TimeLimitToEditPost    { get { return GetConfig("TimeLimitToEditPost"); } }
        public static Config MaxFileSize            { get { return GetConfig("MaxFileSize"); } }
        
        // Registration
        public static Config UsernameMax            { get { return GetConfig("UsernameMax"); } }
        public static Config UsernameMin            { get { return GetConfig("UsernameMin"); } }
        public static Config PasswordMin            { get { return GetConfig("PasswordMin"); } }
        public static Config AccountActivation      { get { return GetConfig("AccountActivation"); } }
        public static Config RegistrationRole       { get { return GetConfig("RegistrationRole"); } }

        // CAPTCHA 
        public static Config CaptchaFontColor       { get { return GetConfig("CaptchaFontColor"); } }
        public static Config CaptchaBackgroundColor { get { return GetConfig("CaptchaBackgroundColor"); } }
        public static Config CaptchaWidth           { get { return GetConfig("CaptchaWidth"); } }
        public static Config CaptchaHeight          { get { return GetConfig("CaptchaHeight"); } }
        public static Config CaptchaFontFamily      { get { return GetConfig("CaptchaFontFamily"); } }
        public static Config CaptchaWarpFactor      { get { return GetConfig("CaptchaWarpFactor"); } }

        private static Config GetConfig(string configName)
        {
            List<Config> configs = (List<Config>)MemoryCache.Default[CacheKeys.SiteConfigurations];

            return configs.Single(x => x.Name == configName);
        }

        public void UpdateCache()
        {
            List<Config> configurations = _configRepository.Get().ToList();

            MemoryCache.Default[CacheKeys.SiteConfigurations] = configurations;
        }
    }
}