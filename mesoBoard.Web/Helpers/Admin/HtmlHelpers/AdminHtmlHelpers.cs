using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

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
        public static IHtmlContent AdminConfirm(this IHtmlHelper html, string linkText, string yesRedirectUrl, string noRedirectUrl, object htmlAttributes = null)
        {
            return html.ActionLink(linkText, "Confirm", "Admin",
            new
            {
                YesRedirect = yesRedirectUrl,
                NoRedirect = noRedirectUrl,
                area = "Admin"
            }, htmlAttributes);
        }

        public static HtmlString AdminDeleteImageLink(this IHtmlHelper html, string YesRedirectUrl, string NoRedirectUrl, string AltText)
        {
            var urlHelperFactory = html.ViewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            var Url = urlHelperFactory.GetUrlHelper(html.ViewContext);

            TagBuilder link = new TagBuilder("a");
            link.MergeAttribute("href", Url.AdminConfirmUrl(YesRedirectUrl, NoRedirectUrl));

            TagBuilder img = new TagBuilder("img");
            img.MergeAttribute("src", Url.Content("~/Areas/Admin/Content/images/delete.png"));
            img.MergeAttribute("alt", AltText);
            img.MergeAttribute("title", AltText);

            link.InnerHtml.AppendHtml(img.ToString());

            return new HtmlString(link.ToString());
        }

        public static IHtmlContent ControllerAwareActionLink(this IHtmlHelper htmlHelper, string linkText, string action, string controller, object htmlAttributes)
        {
            string controllerName = (string)htmlHelper.ViewContext.RouteData.Values["controller"];

            //bool selected = controllerName.Equals(controller, System.StringComparison.InvariantCultureIgnoreCase);

            bool selected = false;
            string breadcrumb = (string)htmlHelper.ViewData[ViewDataKeys.BreadCrumb];
            if (!string.IsNullOrWhiteSpace(breadcrumb))
                selected = breadcrumb.Equals(linkText, System.StringComparison.InvariantCultureIgnoreCase);

            var htmlAttributesDictionary = new RouteValueDictionary(htmlAttributes);

            if (selected)
            {
                string classes = "selected";
                if (htmlAttributesDictionary.ContainsKey("class"))
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