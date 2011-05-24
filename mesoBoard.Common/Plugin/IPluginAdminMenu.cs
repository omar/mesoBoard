namespace mesoBoard.Common
{
    public interface IPluginAdminMenu
    {
        System.Collections.Generic.List<NavigationLink> ChildLinks { get; }
        NavigationLink ParentLink { get; }
    }
}
