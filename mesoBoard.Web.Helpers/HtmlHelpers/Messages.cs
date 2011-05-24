using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using Ninject;
using Ninject.Infrastructure;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        public static IHtmlString PluginConfigValue(this HtmlHelper html, string Name)
        {
            var PluginConfigRep = ((IHaveKernel)html.ViewContext.RequestContext.HttpContext.ApplicationInstance).Kernel.Get<IRepository<PluginConfig>>();
            return new HtmlString(PluginConfigRep.First(item => item.Name.Equals(Name)).Value);
        }

        public static IHtmlString GetMessages(this HtmlHelper html)
        {
            string output = GenerateMessage(ViewDataKeys.GlobalMessages.Success, html); 
            output += GenerateMessage(ViewDataKeys.GlobalMessages.Notice, html);
            output += GenerateMessage(ViewDataKeys.GlobalMessages.Error, html);
            

            return new HtmlString(output);
        }

        private static string GenerateMessage(string messageType, HtmlHelper html)
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
            tag.InnerHtml = message;

            return tag.ToString();
        }

        public static IHtmlString PageTitle(this HtmlHelper html)
        {
            var ConfigRep = ((IHaveKernel)html.ViewContext.RequestContext.HttpContext.ApplicationInstance).Kernel.Get<IRepository<Config>>();
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