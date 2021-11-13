using Microsoft.AspNetCore.Routing;

namespace mesoBoard.Common
{
    public class NavigationLink
    {
        public string Text { get; set; }

        public RouteValueDictionary RouteValue { get; set; }

        public string RouteName { get; set; }

        public object HtmlAttributes { get; set; }
    }
}