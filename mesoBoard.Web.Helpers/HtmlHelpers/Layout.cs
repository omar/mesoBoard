using System.Web;
using System.Web.Mvc;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        public static IHtmlString Spacer(this HtmlHelper html, int height = 1)
        {
            TagBuilder tag = new TagBuilder("div");
            tag.AddCssClass("spacer");
            tag.MergeAttribute("style", string.Format("height: {0}px", height));
            return new HtmlString(tag.ToString());
        }

    }
}
