namespace mesoBoard.Common
{
    public interface IPluginDetails
    {
        string Name { get; }

        string Description { get; }

        string Author { get; }

        string Email { get; }

        string Website { get; }

        IPluginAdminMenu AdminMenu { get; }

        IPluginInstall InstallDetails { get; }

        System.Web.Routing.RouteCollection Routes { get; }

        System.Web.Routing.RouteValueDictionary DefaultRoute { get; }
    }
}