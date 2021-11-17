using System.Web.Mvc;
using System.Web.Mvc.Html;
using mesoBoard.Data;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        public static IHtmlContent CreatePostLink(this IHtmlHelper html, Thread thread)
        {
            string txt = thread.IsLocked ? "Locked" : "Create Post";
            string cssClass = thread.IsLocked ? "img_link create-post-locked" : "img_link create-post";
            if (thread.IsLocked)
                return html.ActionLink(txt, "ViewThread", "Board", new { ThreadID = thread.ThreadID }, new { @class = cssClass });
            else
                return html.ActionLink(txt, "CreatePost", "Post", new { ThreadID = thread.ThreadID }, new { @class = cssClass });
        }

        public static IHtmlContent SubscriptionLink(this IHtmlHelper html, int threadID, bool isSubscribed)
        {
            string txt = isSubscribed ? "Unsubscribe to Thread" : "Subscribe to Thread";
            string cssClass = "img_link thread-subscription";

            if (isSubscribed)
                return html.ActionLink(txt, "UnsubscribeToThread", new { ThreadID = threadID }, new { @class = cssClass });
            else
                return html.ActionLink(txt, "SubscribeToThread", new { ThreadID = threadID }, new { @class = cssClass });
        }
    }
}