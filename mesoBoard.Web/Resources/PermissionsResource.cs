using Microsoft.Extensions.Localization;

namespace mesoBoard.Web.Resources
{
    public class PermissionsResource
    {
        private readonly IStringLocalizer<PermissionsResource> _localizer;

        public PermissionsResource(IStringLocalizer<PermissionsResource> localizer) =>
            _localizer = localizer;

        public string Attachments_Download => GetString(nameof(Attachments_Download));
        public string Attachments_None => GetString(nameof(Attachments_None));
        public string Attachments_Upload => GetString(nameof(Attachments_Upload));
        public string Polling_Create => GetString(nameof(Polling_Create));
        public string Polling_None => GetString(nameof(Polling_None));
        public string Polling_Vote => GetString(nameof(Polling_Vote));
        public string Posting_Announcments => GetString(nameof(Posting_Announcments));
        public string Posting_None => GetString(nameof(Posting_None));
        public string Posting_Reply => GetString(nameof(Posting_Reply));
        public string Posting_Sticky => GetString(nameof(Posting_Sticky));
        public string Posting_Thread => GetString(nameof(Posting_Thread));
        public string Visibility_Hidden => GetString(nameof(Visibility_Hidden));
        public string Visibility_Visible => GetString(nameof(Visibility_Visible));

        private string GetString(string name) =>
            _localizer[name];
    }
}