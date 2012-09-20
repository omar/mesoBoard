using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using mesoBoard.Common;
using mesoBoard.Data;
using Ninject;
using Ninject.Infrastructure;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        public static int GetNewMessagesCount(this HtmlHelper html)
        {
            return (int)html.ViewData["mbNewMessagesCount"];
        }

        public static MvcHtmlString UsernameLink(this HtmlHelper html, User user, bool includeRankColor = true)
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

        public static IHtmlString UsernameColor(this HtmlHelper html, User user)
        {
            if (user == null || user.UserID == 0)
                return new HtmlString(string.Empty);

            var kernel = ((IHaveKernel)html.ViewContext.RequestContext.HttpContext.ApplicationInstance).Kernel;
            var RoleRep = kernel.Get<IRepository<Role>>();

            if (user.UserProfile.DefaultRole.HasValue)
            {
                Role role = RoleRep.Get(user.UserProfile.DefaultRole.Value);
                return new HtmlString(role.Rank.Color);
            }
            else
                return new HtmlString(string.Empty);
        }

        public static IHtmlString UserRank(this HtmlHelper html, User user)
        {
            Rank rank;

            var kernel = ((IHaveKernel)html.ViewContext.RequestContext.HttpContext.ApplicationInstance).Kernel;
            var rolesRepository = kernel.Get<IRepository<Role>>();
            var rankRepository = kernel.Get<IRepository<Rank>>();

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
                return new HtmlString(title.ToString());

            UrlHelper url = new UrlHelper(html.ViewContext.RequestContext);

            string src = string.Format("~/Images/Ranks/{0}", rank.Image);

            TagBuilder image = new TagBuilder("img");
            image.MergeAttribute("src", url.Content(src));

            HtmlString output = new HtmlString(title.ToString() + image.ToString(TagRenderMode.SelfClosing));

            return output;
        }
    }
}