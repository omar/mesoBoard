namespace mesoBoard.Common
{
    public interface IPluginInstall
    {
        System.Collections.Generic.List<PluginConfiguration> GetConfigs();

        string GetSQL { get; }
    }
}