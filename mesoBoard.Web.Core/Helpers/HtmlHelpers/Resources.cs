using mesoBoard.Web.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace mesoBoard.Web.Helpers
{
    public static class Resources
    {
        public static IHtmlContent PermissionResource(this IHtmlHelper htmlHelper, string key)
        {
            var localizer = htmlHelper.GetService<IStringLocalizer<PermissionsResource>>();
            return new HtmlString(localizer.GetString(key));
        }
    }
}