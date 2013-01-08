using System.Web.Mvc;

namespace mesoBoard.Web.Helpers
{
    public static class AdminUrlHelpers
    {
        public static string AdminConfirmUrl(this UrlHelper url, string YesRedirectUrl, string NoRedirectUrl)
        {
            return url.Action("Confirm", "Admin", new
            {
                YesRedirect = YesRedirectUrl,
                NoRedirect = NoRedirectUrl,
                area = "Admin"
            });
        }
    }
}