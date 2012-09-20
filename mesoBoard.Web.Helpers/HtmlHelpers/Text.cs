using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Common;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        public static IHtmlString SyntaxHighlightingScripts(this HtmlHelper html, params SyntaxHighlighting[] langs)
        {
            UrlHelper Url = new UrlHelper(html.ViewContext.RequestContext);

            StringBuilder str = new StringBuilder();

            string javascript = "<script language='javascript' src='{0}' ></script>";

            str.AppendLine(string.Format(javascript, Url.ThemeContent("Content/SyntaxHighlighter/Scripts/shCore.js")));

            foreach (var l in langs)
            {
                string url = string.Format(Url.ThemeContent("Content/SyntaxHighlighter/Scripts/shBrush{0}.js"), l.ToString());
                str.AppendLine(string.Format(javascript, url));
            }

            string test = str.ToString();

            return new HtmlString(str.ToString());
        }

        public static IHtmlString SyntaxHighlightingScripts(this HtmlHelper html, string commaDelimitedList)
        {
            string[] split = commaDelimitedList.Split(',');

            List<SyntaxHighlighting> langs = new List<SyntaxHighlighting>();

            foreach (string s in split)
            {
                langs.Add((SyntaxHighlighting)Enum.Parse(typeof(SyntaxHighlighting), s));
            }

            return SyntaxHighlightingScripts(html, langs.ToArray());
        }

        public static decimal PercentOfVotes(this HtmlHelper html, int voteCount, int totalVotes)
        {
            if (totalVotes == 0)
                return 0;

            decimal ratio = (decimal)voteCount / (decimal)totalVotes;
            decimal percent = ratio * 100;

            return Math.Round(percent);
        }

        public static IHtmlString VoteBar(this HtmlHelper html, int voteCount, int totalVotes)
        {
            decimal percent = PercentOfVotes(html, voteCount, totalVotes) / 2;

            TagBuilder tag = new TagBuilder("div");
            tag.AddCssClass("vote_bar");
            tag.InnerHtml = "&nbsp;";
            tag.Attributes.Add("style", "width:" + percent.ToString() + "%");

            return new HtmlString(tag.ToString());
        }

        public static IHtmlString FriendlyFileSize(this HtmlHelper html, int fileSizeBytes)
        {
            string unit = "bytes";
            double ratio = fileSizeBytes;
            if (fileSizeBytes < 1024)
            {
                unit = "bytes";
                ratio = fileSizeBytes;
            }
            else if (fileSizeBytes < Math.Pow(1024, 2))
            {
                unit = "kb";
                ratio = (fileSizeBytes / Math.Pow(1024, 1));
            }
            else if (fileSizeBytes < Math.Pow(1024, 3))
            {
                unit = "mb";
                ratio = (fileSizeBytes / Math.Pow(1024, 2));
            }
            else
            {
                unit = "gb";
                ratio = (fileSizeBytes / Math.Pow(1024, 3));
            }

            return MvcHtmlString.Create(string.Format("{0} {1}", ratio.ToString("#.##"), unit));
        }

        public static IHtmlString SubmitButtonText(this HtmlHelper htmlHelper, int idValue)
        {
            if (idValue == 0)
                return MvcHtmlString.Create("Create");
            else
                return MvcHtmlString.Create("Save Changes");
        }
    }
}