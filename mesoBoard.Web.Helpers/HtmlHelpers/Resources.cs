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
        public static IHtmlString PermissionResource(this HtmlHelper htmlHelper, string key)
        {
            string resourceClass = "Permissions";
            return htmlHelper.Resource(resourceClass, key);
        }

        public static IHtmlString Resource(this HtmlHelper htmlHelper, string resourceClass, string key)
        {
            string value = (string)HttpContext.GetGlobalResourceObject(resourceClass, key);

            return MvcHtmlString.Create(value);
        }
    }
}