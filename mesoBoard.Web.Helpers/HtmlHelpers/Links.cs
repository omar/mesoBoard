using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        public static MvcHtmlString ConfirmLink(this HtmlHelper html, string linkText, string yesRedirectUrl, string noRedirectUrl, object htmlAttributes = null)
        {
            return html.ActionLink(linkText, "Confirm", "Board",
            new
            {
                YesRedirect = yesRedirectUrl,
                NoRedirect = noRedirectUrl,
            }, htmlAttributes);
        }

        public static MvcHtmlString TopBreadCrumbLink(this HtmlHelper html, object htmlAttributes = null)
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
                return html.ActionLink(text,"Index", routeValues, new RouteValueDictionary(htmlAttributes));
            }
        }

        public static MvcHtmlString BreadCrumbLink(this HtmlHelper html, object htmlAttributes = null)
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
                string text = (string)html.ViewData[ViewDataKeys.BreadCrumb] ;
                return html.ActionLink(text, action, routeValues, new RouteValueDictionary(htmlAttributes));
            }
        }

        public static IHtmlString UserProfileLink(this HtmlHelper html, string UserNameOrID)
        {
            UrlHelper Url = new UrlHelper(html.ViewContext.RequestContext);
            TagBuilder link = new TagBuilder("a");
            link.Attributes.Add("href", Url.Action("UserProfile", "Members", new { UserNameOrID = UserNameOrID }));
            link.Attributes.Add("title", "User Profile");
            link.InnerHtml = UserNameOrID;
            return new HtmlString(link.ToString());
        }

        public static IHtmlString ImageLink(this HtmlHelper html, string text, string link,
            string imageSource, string titleAndAlt, string cssClass)
        {
            UrlHelper Url = new UrlHelper(html.ViewContext.RequestContext);
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
