using System;
using System.IO;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        //
        // CAPTCHA code taken from "Pro ASP.NET MVC Framework" by Steven Sanderson
        // Apress publishing
        //
        // Modified for mesoBoard

        public static IHtmlContent Captcha(this IHtmlHelper html, string hiddenFormFieldName = "captcha_code")
        {
            string challengeGuid = Guid.NewGuid().ToString();

            html.ViewContext.HttpContext.Session.SetString(SessionKeys.CaptchaSessionPrefix + challengeGuid, MakeRandomSolution());

            var urlHelper = html.GetUrlHelper();
            string url = urlHelper.Action(new UrlActionContext()
            {
                Action = "Render",
                Controller =  "Captcha",
                Values = new { challengeGuid }
            });

            TagBuilder image = new TagBuilder("img");
            image.MergeAttribute("src", url);
            image.MergeAttribute("alt", "CAPTCHA");
            image.TagRenderMode = TagRenderMode.SelfClosing;

            TagBuilder hidden = new TagBuilder("input");
            hidden.MergeAttribute("type", "hidden");
            hidden.MergeAttribute("value", challengeGuid);
            hidden.MergeAttribute("name", hiddenFormFieldName);
            hidden.TagRenderMode = TagRenderMode.SelfClosing;

            return new HtmlString(image.ToString() + hidden.ToString());
        }

        public static IHtmlContent FileTypeImage(this IHtmlHelper html, string fileExtension)
        {
            var FileTypes = html.ViewContext.HttpContext.RequestServices.GetRequiredService<IRepository<FileType>>();
            
            FileType type = FileTypes.First(item => item.Extension.Equals(fileExtension));

            if (type == null)
                return new HtmlString("");

            var ConfigRep = html.ViewContext.HttpContext.RequestServices.GetRequiredService<IRepository<Config>>();
            TagBuilder tag = new TagBuilder("img");
            var Url = html.GetUrlHelper();
            string src = Url.Content(System.IO.Path.Combine(DirectoryPaths.FileTypes, type.Image));
            tag.Attributes.Add("src", src);
            tag.Attributes.Add("alt", type.Extension);
            tag.Attributes.Add("title", type.Extension);
            return new HtmlString(tag.ToString());
        }

        public static IHtmlContent ThemeFolder(this IHtmlHelper html)
        {
            return new HtmlString((string)html.ViewContext.HttpContext.Items[HttpContextItemKeys.ThemeFolder]);
        }

        public static IHtmlContent Image(this IHtmlHelper html, string imageSource, object htmlAttributes = null)
        {
            var url = html.GetUrlHelper();
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

            return new HtmlString(tag.ToString());
        }

        public static IHtmlContent ThemeImage(this IHtmlHelper html, string themeImageFileName)
        {
            return html.Image("~/Themes/" + html.ThemeFolder() + "/Images/" + themeImageFileName);
        }

        public static IHtmlContent ThemeImage(this IHtmlHelper html, string themeImageFileName, string altText)
        {
            return html.Image("~/Themes/" + html.ThemeFolder() + "/Images/" + themeImageFileName, altText);
        }

        public static IHtmlContent ThemeImage(this IHtmlHelper html, string themeImageFileName, object htmlAttributes)
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

        public static IHtmlContent Label(this IHtmlHelper html, string forControl, string innerHTML, string cssClass = "")
        {
            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add("for", forControl);
            if (!string.IsNullOrWhiteSpace(cssClass))
                tag.Attributes.Add("class", cssClass);
            tag.InnerHtml.AppendHtml(innerHTML);
            return tag.ToString().ToHtmlString();
        }

        public static IHtmlContent UserAvatar(this IHtmlHelper html, int UserID)
        {
            var UserRep = html.ViewContext.HttpContext.RequestServices.GetRequiredService<IRepository<User>>();

            User user = UserRep.Get(UserID);
            UserProfile profile = user.UserProfile;

            if (profile.AvatarType == "None" || string.IsNullOrEmpty(user.UserProfile.Avatar))
                return "".ToHtmlString();

            var Url = html.GetUrlHelper();

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

        public static IHtmlContent ParseNavLink(this IHtmlHelper html, NavigationLink link)
        {
            if (link == null)
                return "".ToHtmlString();

            var url = html.GetUrlHelper();
            TagBuilder linktag = new TagBuilder("a");

            string breadCrumb = html.ViewContext.TempData[ViewDataKeys.BreadCrumb] as string ?? html.ViewContext.ViewData[ViewDataKeys.BreadCrumb] as string
                                ?? html.ViewContext.TempData[ViewDataKeys.TopBreadCrumb] as string ?? html.ViewContext.ViewData[ViewDataKeys.TopBreadCrumb] as string;

            RouteValueDictionary htmlAttributes = new RouteValueDictionary(link.HtmlAttributes);
            linktag.MergeAttributes(htmlAttributes);

            if (!string.IsNullOrWhiteSpace(breadCrumb))
                if (link.Text == breadCrumb)
                    linktag.AddCssClass("selected");

            if (link.RouteValue != null)
                linktag.Attributes.Add("href", url.Link(link.RouteName, link.RouteValue));
            else
                linktag.Attributes.Add("href", url.Link(link.RouteName, null));

            linktag.InnerHtml.AppendHtml(link.Text);
            return linktag.ToString().ToHtmlString();
        }

        public static IHtmlContent RankImage(this IHtmlHelper html, string source)
        {
            var url = html.GetUrlHelper();
            string src = Path.Combine(DirectoryPaths.Ranks, source);

            TagBuilder imageTag = new TagBuilder("img");
            imageTag.MergeAttribute("src", url.Content(src));

            return new HtmlString(imageTag.ToString());
        }
    }
}