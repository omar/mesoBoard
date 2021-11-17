using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace mesoBoard.Web.Helpers
{
    public static class Resources
    {
        public static IHtmlContent PermissionResource(this IHtmlHelper htmlHelper, string key)
        {
            string resourceClass = "Permissions";
            return htmlHelper.Resource(resourceClass, key);
        }

        public static IHtmlContent Resource(this IHtmlHelper htmlHelper, string resourceClass, string key)
        {
            string value = (string)HttpContext.GetGlobalResourceObject(resourceClass, key);

            return new HtmlString(value);
        }
    }
}