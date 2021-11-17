using System;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        public static IHtmlContent PluginConfigValue(this IHtmlHelper html, string Name)
        {
            var PluginConfigRep = html.GetService<IRepository<PluginConfig>>();
            return new HtmlString(PluginConfigRep.First(item => item.Name.Equals(Name)).Value);
        }

        public static IHtmlContent GetMessages(this IHtmlHelper html)
        {
            string output = GenerateMessage(ViewDataKeys.GlobalMessages.Success, html);
            output += GenerateMessage(ViewDataKeys.GlobalMessages.Notice, html);
            output += GenerateMessage(ViewDataKeys.GlobalMessages.Error, html);

            return new HtmlString(output);
        }

        private static string GenerateMessage(string messageType, IHtmlHelper html)
        {
            string message = (string)html.ViewContext.TempData[messageType] ?? (string)html.ViewData[messageType] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;

            string cssClass = string.Empty;

            if (messageType == ViewDataKeys.GlobalMessages.Error)
                cssClass = "error";
            else if (messageType == ViewDataKeys.GlobalMessages.Notice)
                cssClass = "notice";
            else if (messageType == ViewDataKeys.GlobalMessages.Success)
                cssClass = "success";

            TagBuilder tag = new TagBuilder("div");
            tag.AddCssClass(cssClass);
            tag.AddCssClass("global_message");
            tag.InnerHtml.SetHtmlContent(message);

            return tag.ToString();
        }

        public static IHtmlContent PageTitle(this IHtmlHelper html)
        {
            var ConfigRep = html.GetService<IRepository<Config>>();
            string breadCrumb = html.ViewData[ViewDataKeys.BreadCrumb] as string;
            if (!string.IsNullOrWhiteSpace(breadCrumb))
            {
                string TheCrumb = html.ViewData[ViewDataKeys.BreadCrumb].ToString();
                string[] seperators = { " | " };
                string[] Split = TheCrumb.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                return new HtmlString(Split.Last() + ConfigRep.First(item => item.Name.Equals("PageTitlePhrase")).Value);
            }
            else
            {
                Config boardName = ConfigRep.First(item => item.Name.Equals("BoardName"));
                Config titlePhrase = ConfigRep.First(item => item.Name.Equals("PageTitlePhrase"));
                return new HtmlString(string.Format("{0} {1}", boardName.Value, titlePhrase.Value));
            }
        }
    }
}