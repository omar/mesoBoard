using System;
using System.Web;
using System.Web.Routing;
using mesoBoard.Common;
using mesoBoard.Data;
using Ninject;
using Ninject.Infrastructure;
using System.IO;
using System.Web.Mvc;
using mesoBoard.Services;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        //
        // CAPTCHA code taken from "Pro ASP.NET MVC Framework" by Steven Sanderson
        // Apress publishing
        //  
        // Modified for mesoBoard

        public static IHtmlString Captcha(this HtmlHelper html, string hiddenFormFieldName = "captcha_code")
        {
            string challengeGuid = Guid.NewGuid().ToString();
            
            html.ViewContext.HttpContext.Session[SessionKeys.CaptchaSessionPrefix + challengeGuid] = MakeRandomSolution();
            
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string url = urlHelper.Action("Render", "Captcha", new { challengeGuid });

            TagBuilder image = new TagBuilder("img");
            image.MergeAttribute("src", url);
            image.MergeAttribute("alt", "CAPTCHA");

            TagBuilder hidden = new TagBuilder("input");
            hidden.MergeAttribute("type", "hidden");
            hidden.MergeAttribute("value", challengeGuid);
            hidden.MergeAttribute("name", hiddenFormFieldName);

            return new HtmlString(image.ToString(TagRenderMode.SelfClosing) + hidden.ToString(TagRenderMode.SelfClosing));
        }

        public static IHtmlString FileTypeImage(this HtmlHelper html, string fileExtension)
        {
            var kernel = ((IHaveKernel)html.ViewContext.RequestContext.HttpContext.ApplicationInstance).Kernel;
            var FileTypes = kernel.Get<IRepository<FileType>>();

            FileType type = FileTypes.First(item => item.Extension.Equals(fileExtension));

            if (type == null)
                return new HtmlString("");

            var ConfigRep = kernel.Get<IRepository<Config>>();
            TagBuilder tag = new TagBuilder("img");
            UrlHelper Url = new UrlHelper(html.ViewContext.RequestContext);
            string src = Url.Content(System.IO.Path.Combine(DirectoryPaths.FileTypes, type.Image));
            tag.Attributes.Add("src", src);
            tag.Attributes.Add("alt", type.Extension);
            tag.Attributes.Add("title", type.Extension);
            return new HtmlString(tag.ToString());
        }

        public static MvcHtmlString ThemeFolder(this HtmlHelper html)
        {
            return MvcHtmlString.Create((string)html.ViewContext.HttpContext.Items[HttpContextItemKeys.ThemeFolder]);
        }

        public static MvcHtmlString Image(this HtmlHelper html, string imageSource, object htmlAttributes = null)
        {
            UrlHelper url = new UrlHelper(html.ViewContext.RequestContext);
            TagBuilder tag = new TagBuilder("img");
            string src = url.Content(imageSource);
            tag.Attributes.Add("src", src);

            if (htmlAttributes != null)
            {
                RouteValueDictionary attributes = new RouteValueDictionary(htmlAttributes);
                foreach (var item in attributes)
                {
                    tag.Attributes.Add(item.Key, item.Value as string);
                }
            }

            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString ThemeImage(this HtmlHelper html, string themeImageFileName)
        {
            return html.Image("~/Themes/"+html.ThemeFolder() + "/Images/" + themeImageFileName);
        }

        public static MvcHtmlString ThemeImage(this HtmlHelper html, string themeImageFileName, string altText)
        {
            return html.Image("~/Themes/" + html.ThemeFolder() + "/Images/" + themeImageFileName, altText);
        }

        public static MvcHtmlString ThemeImage(this HtmlHelper html, string themeImageFileName, object htmlAttributes)
        {
            return html.Image("~/Themes/" + html.ThemeFolder() + "/Images/" + themeImageFileName, (object)htmlAttributes);
        }

        private static string MakeRandomSolution()
        {
            Random rng = new Random();
            int length = rng.Next(5, 7);
            char[] buf = new char[length];
            for (int i = 0; i < length; i++)
                buf[i] = (char)('a' + rng.Next(26));
            return new string(buf);
        }

        public static IHtmlString Label(this HtmlHelper html, string forControl, string innerHTML, string cssClass="")
        {
            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add("for", forControl);
            if(!string.IsNullOrWhiteSpace(cssClass))
                tag.Attributes.Add("class", cssClass);
            tag.InnerHtml = innerHTML;
            return tag.ToString().ToHtmlString();
        }

        public static IHtmlString UserAvatar(this HtmlHelper html, int UserID)
        {
            var kernel = ((IHaveKernel)html.ViewContext.RequestContext.HttpContext.ApplicationInstance).Kernel;
            var UserRep = kernel.Get<IRepository<User>>();

            User user = UserRep.Get(UserID);
            UserProfile profile = user.UserProfile;

            if (profile.AvatarType == "None" || string.IsNullOrEmpty(user.UserProfile.Avatar))
                return "".ToHtmlString();

            UrlHelper Url = new UrlHelper(html.ViewContext.RequestContext);

            TagBuilder imgtag = new TagBuilder("img");

            imgtag.Attributes.Add("alt", user.Username + "'s Avatar");
            imgtag.Attributes.Add("title", user.Username + "'s Avatar");

            string src;
            if (profile.AvatarType == "Upload")
                src = Url.Content(System.IO.Path.Combine(DirectoryPaths.Avatars, profile.Avatar));
            else
            {
                src = profile.Avatar;
                imgtag.MergeAttribute("width", SiteConfig.AvatarWidth.ToString());
                imgtag.MergeAttribute("height", SiteConfig.AvatarHeight.ToString());
            }

            imgtag.Attributes.Add("src", src);
            
            return imgtag.ToString().ToHtmlString();
        }

        public static IHtmlString ParseNavLink(this HtmlHelper html, NavigationLink link)
        {
            if (link == null)
                return "".ToHtmlString();

            UrlHelper url = new UrlHelper(html.ViewContext.RequestContext);
            TagBuilder linktag = new TagBuilder("a");

            string breadCrumb = html.ViewContext.TempData[ViewDataKeys.BreadCrumb] as string ?? html.ViewContext.ViewData[ViewDataKeys.BreadCrumb] as string
                                ?? html.ViewContext.TempData[ViewDataKeys.TopBreadCrumb] as string ?? html.ViewContext.ViewData[ViewDataKeys.TopBreadCrumb] as string;

            RouteValueDictionary htmlAttributes = new RouteValueDictionary(link.HtmlAttributes);
            linktag.MergeAttributes(htmlAttributes);

            if (!string.IsNullOrWhiteSpace(breadCrumb))
                if (link.Text == breadCrumb)
                    linktag.AddCssClass("selected");
            
            if (link.RouteValue != null)
                linktag.Attributes.Add("href", url.RouteUrl(link.RouteValue));
            else
                linktag.Attributes.Add("href", url.RouteUrl(link.RouteName));

            linktag.InnerHtml += link.Text;
            return linktag.ToString().ToHtmlString();
        }

        public static IHtmlString RankImage(this HtmlHelper html, string source)
        {
            UrlHelper url = new UrlHelper(html.ViewContext.RequestContext);
            string src = Path.Combine(DirectoryPaths.Ranks, source);
            
            TagBuilder imageTag = new TagBuilder("img");
            imageTag.MergeAttribute("src", url.Content(src));

            return MvcHtmlString.Create(imageTag.ToString());
        }

    }
}
