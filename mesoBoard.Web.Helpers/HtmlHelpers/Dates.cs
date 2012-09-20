using System;
using System.Web;
using System.Web.Mvc;

namespace mesoBoard.Web.Helpers
{
    public static partial class mesoBoardHtmlHelpers
    {
        public static DateTime TimeZoneOffset(this HtmlHelper html, DateTime date)
        {
            int offset = (int)html.ViewData[ViewDataKeys.TimeZoneOffset];
            return date.AddHours(offset);
        }

        public static IHtmlString RelativeDate(this HtmlHelper html, DateTime CompleteDate, int TimeOffset = 0)
        {
            // From http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time/1248#1248
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - CompleteDate.Ticks);
            ts = ts.Add(TimeSpan.FromHours(TimeOffset));
            double delta = ts.TotalSeconds;
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            string output;

            if (delta < 1 * MINUTE)
            {
                output = ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            else if (delta < 2 * MINUTE)
            {
                output = "a minute ago";
            }
            else if (delta < 45 * MINUTE)
            {
                output = ts.Minutes + " minutes ago";
            }
            else if (delta < 90 * MINUTE)
            {
                output = "an hour ago";
            }
            else if (delta < 24 * HOUR)
            {
                output = ts.Hours + " hours ago";
            }
            else if (delta < 48 * HOUR)
            {
                output = "yesterday";
            }
            else if (delta < 30 * DAY)
            {
                output = ts.Days + " days ago";
            }
            else if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                output = months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                output = years <= 1 ? "one year ago" : years + " years ago";
            }

            return new HtmlString(output);
        }
    }
}