using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace mesoBoard.Common
{
    public static class TagBuilderExtenstions
    {
        public static string WriteToString(this IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}