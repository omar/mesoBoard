using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web;
using System.Web.Routing;

namespace mesoBoard.Web.Helpers
{
    public static class AdminHtmlHelpers
    {
        /// <summary>
        /// Creates a link to the admin operation confirmation page.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <param name="YesRedirectURL">URL to redirect to for YES response.</param>
        /// <param name="noRedirectUrl">URL to redirect to for NO response.</param>
        /// <returns></returns>
        public static MvcHtmlString AdminConfirm(this HtmlHelper html, string linkText, string yesRedirectUrl, string noRedirectUrl, object htmlAttributes = null)
        {
            return html.ActionLink(linkText, "Confirm", "Admin",
            new
            {
                YesRedirect = yesRedirectUrl,
                NoRedirect = noRedirectUrl,
                area = "Admin"
            }, htmlAttributes);
        }

        public static MvcHtmlString AdminDeleteImageLink(this HtmlHelper html, string YesRedirectUrl, string NoRedirectUrl, string AltText)
        {
            UrlHelper Url = new UrlHelper(html.ViewContext.RequestContext);

            TagBuilder link = new TagBuilder("a");
            link.MergeAttribute("href", Url.AdminConfirmUrl(YesRedirectUrl, NoRedirectUrl));

            TagBuilder img = new TagBuilder("img");
            img.MergeAttribute("src", Url.Content("~/Areas/Admin/Content/images/delete.png"));
            img.MergeAttribute("alt", AltText);
            img.MergeAttribute("title", AltText);

            link.InnerHtml = img.ToString();

            return MvcHtmlString.Create(link.ToString());
        }

        public static IHtmlString ControllerAwareActionLink(this HtmlHelper htmlHelper, string linkText, string action, string controller, object htmlAttributes)
        {
            string controllerName = (string)htmlHelper.ViewContext.RouteData.Values["controller"];
            //bool selected = controllerName.Equals(controller, System.StringComparison.InvariantCultureIgnoreCase);

            bool selected = false;
            string breadcrumb = (string)htmlHelper.ViewData[ViewDataKeys.BreadCrumb];
            if (!string.IsNullOrWhiteSpace(breadcrumb))
                selected = breadcrumb.Equals(linkText, System.StringComparison.InvariantCultureIgnoreCase);

            RouteValueDictionary htmlAttributesDictionary = new RouteValueDictionary(htmlAttributes);

            if (selected)
            {
                string classes = "selected";
                if(htmlAttributesDictionary.ContainsKey("class"))
                {
                    classes = (string)htmlAttributesDictionary["class"];
                    classes += " selected";
                }

                htmlAttributesDictionary["class"] = classes;
            }

            RouteValueDictionary routes = new RouteValueDictionary(new { area = "Admin" });

            return htmlHelper.ActionLink(linkText, action, controller, routes, htmlAttributesDictionary);
        }
    }
}