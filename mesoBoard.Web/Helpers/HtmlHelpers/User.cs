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
        public static int GetNewMessagesCount(this IHtmlHelper html)
        {
            return (int)html.ViewData["mbNewMessagesCount"];
        }

        public static IHtmlContent UsernameLink(this IHtmlHelper html, User user, bool includeRankColor = true)
        {
            if (includeRankColor)
            {
                string colorHex = html.UsernameColor(user).ToString();
                object styleObject = colorHex != string.Empty ? new { style = colorHex } : null;
                return html.ActionLink(user.Username, "UserProfile", "Members", new { UserNameOrID = user.UserID }, styleObject);
            }
            else
                return html.ActionLink(user.Username, "UserProfile", "Members", new { UserNameOrID = user.UserID }, null);
        }

        public static IHtmlContent UsernameColor(this IHtmlHelper html, User user)
        {
            if (user == null || user.UserID == 0)
                return new HtmlString(string.Empty);

            var RoleRep = html.GetService<IRepository<Role>>();

            if (user.UserProfile.DefaultRole.HasValue)
            {
                Role role = RoleRep.Get(user.UserProfile.DefaultRole.Value);
                return new HtmlString(role.Rank.Color);
            }
            else
                return new HtmlString(string.Empty);
        }

        public static IHtmlContent UserRank(this IHtmlHelper html, User user)
        {
            Rank rank;

            var rolesRepository = html.GetService<IRepository<Role>>();
            var rankRepository = html.GetService<IRepository<Rank>>();

            if (user.UserProfile.DefaultRole.HasValue)
            {
                Role role = rolesRepository.Get(user.UserProfile.DefaultRole.Value);
                rank = role.Rank;
            }
            else
                rank = rankRepository.Where(item => item.PostCount - user.Posts.Count < 0).OrderByDescending(item => item.PostCount).FirstOrDefault();

            if (rank == null)
                return null;

            TagBuilder title = new TagBuilder("span");
            title.MergeAttribute("style", string.Format("color:{0}", rank.Color));
            title.AddCssClass("title");

            if (string.IsNullOrWhiteSpace(rank.Image))
                return title;

            var url = html.GetUrlHelper();

            string src = string.Format("~/Images/Ranks/{0}", rank.Image);

            TagBuilder image = new TagBuilder("img");
            image.MergeAttribute("src", url.Content(src));
            
            var output = new HtmlContentBuilder();
            output.AppendHtml(title);
            output.AppendHtml(image);

            return output;
        }
    }
}