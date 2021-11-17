using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace mesoBoard.Web.Helpers
{
    public static class UtilityExtensionMethods
    {
        public static IUrlHelper GetUrlHelper(this IHtmlHelper html)
        {
            var urlHelperFactory = html.ViewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = urlHelperFactory.GetUrlHelper(html.ViewContext);
            return urlHelper;
        }

        public static T GetService<T>(this IHtmlHelper html)
        {
            return html.ViewContext.HttpContext.RequestServices.GetRequiredService<T>();
        }
    }
}