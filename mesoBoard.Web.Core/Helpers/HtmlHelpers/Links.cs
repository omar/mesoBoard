using mesoBoard.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        public static IHtmlContent ConfirmLink(this IHtmlHelper html, string linkText, string yesRedirectUrl, string noRedirectUrl, object htmlAttributes = null)
        {
            return html.ActionLink(linkText, "Confirm", "Board",
            new
            {
                YesRedirect = yesRedirectUrl,
                NoRedirect = noRedirectUrl,
            }, htmlAttributes);
        }

        public static IHtmlContent TopBreadCrumbLink(this IHtmlHelper html, object htmlAttributes = null)
        {
            string action = (string)html.ViewContext.RouteData.Values["action"];
            string controller = (string)html.ViewContext.RouteData.Values["controller"];
            var routeValues = html.ViewContext.RouteData.Values;

            if (html.ViewData[ViewDataKeys.TopBreadCrumb] == null || string.IsNullOrWhiteSpace((string)html.ViewData[ViewDataKeys.TopBreadCrumb]))
            {
                return html.ActionLink(controller.SeperateWords(), "Index", routeValues, new RouteValueDictionary(htmlAttributes));
            }
            else
            {
                string text = (string)html.ViewData[ViewDataKeys.TopBreadCrumb];
                return html.ActionLink(text, "Index", routeValues, new RouteValueDictionary(htmlAttributes));
            }
        }

        public static IHtmlContent BreadCrumbLink(this IHtmlHelper html, object htmlAttributes = null)
        {
            string action = (string)html.ViewContext.RouteData.Values["action"];
            string controller = (string)html.ViewContext.RouteData.Values["controller"];
            var routeValues = html.ViewContext.RouteData.Values;

            if (html.ViewData[ViewDataKeys.BreadCrumb] == null || string.IsNullOrWhiteSpace((string)html.ViewData[ViewDataKeys.BreadCrumb]))
            {
                return html.ActionLink(action.SeperateWords(), action, routeValues, new RouteValueDictionary(htmlAttributes));
            }
            else
            {
                string text = (string)html.ViewData[ViewDataKeys.BreadCrumb];
                return html.ActionLink(text, action, routeValues, new RouteValueDictionary(htmlAttributes));
            }
        }

        public static IHtmlContent UserProfileLink(this IHtmlHelper html, string UserNameOrID)
        {
            var Url = html.GetUrlHelper();
            TagBuilder link = new TagBuilder("a");
            link.Attributes.Add("href", Url.Action("UserProfile", "Members", new { UserNameOrID = UserNameOrID }));
            link.Attributes.Add("title", "User Profile");
            link.InnerHtml.AppendHtml(UserNameOrID);
            return link;
        }

        public static IHtmlContent ImageLink(this IHtmlHelper html, string text, string link,
            string imageSource, string titleAndAlt, string cssClass)
        {
            var Url = html.GetUrlHelper();
            link = Url.Content(link);
            if (cssClass == "image_link")
            {
                return new HtmlString(string.Format("<a href='{0}' title='{2}' class='{4}'><img src='{1}' alt='{2}' title='{2}' />{3}</a>",
                    link,
                    Url.Content(imageSource),
                    titleAndAlt,
                    text,
                    cssClass));
            }
            else
            {
                return new HtmlString(string.Format("<a href='{0}' title='{2}' class='{4}'><img src='{1}' alt='{2}' title='{2}' /></a><a href='{0}' title='{2}' class='{4}'>{3}</a>",
                    link,
                    Url.Content(imageSource),
                    titleAndAlt,
                    text,
                    cssClass));
            }
        }
    }
}