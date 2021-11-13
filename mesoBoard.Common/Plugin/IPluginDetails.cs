using Microsoft.AspNetCore.Routing;

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

        RouteCollection Routes { get; }

        RouteValueDictionary DefaultRoute { get; }
    }
}