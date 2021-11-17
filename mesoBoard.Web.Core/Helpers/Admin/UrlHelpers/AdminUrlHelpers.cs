using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace mesoBoard.Web.Helpers
{
    public static class AdminUrlHelpers
    {
        public static string AdminConfirmUrl(this IUrlHelper url, string YesRedirectUrl, string NoRedirectUrl)
        {
            return url.Action(new UrlActionContext()
            {
                Action = "Confirm",
                Controller = "Admin",
                Values = new
                {
                    YesRedirect = YesRedirectUrl,
                    NoRedirect = NoRedirectUrl,
                    area = "Admin"
                }
            });
        }
    }
}