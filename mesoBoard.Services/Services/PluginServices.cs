using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class PluginServices : BaseService
    {
        private IRepository<Plugin> Plugins;

        public PluginServices(
            IRepository<Plugin> plugins,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this.Plugins = plugins;
        }

        public List<Plugin> GetPluginsInFolder(string folderPath)
        {
            List<IPluginDetails> pluginDetails = GetPluginDetailsInFolder(folderPath);

            List<Plugin> plugins = new List<Plugin>();

            foreach (var details in pluginDetails)
            {
                plugins.Add(new Plugin
                {
                    Name = details.Name,
                    Description = details.Description,
                    AssemblyName = details.ToString().Replace(".PluginDetails", ""),
                    Installed = PluginInstalled(details.Name)
                });
            }

            return plugins;
        }

        public bool PluginInstalled(string pluginName)
        {
            return Plugins.First(item => item.Name.Equals(pluginName)) != null;
        }

        public List<IPluginDetails> GetPluginDetailsInFolder(string folderPath)
        {
            string[] pluginAssemblies = Directory.GetFiles(folderPath, "*.dll");

            List<IPluginDetails> plugins = new List<IPluginDetails>();

            foreach (string plugin in pluginAssemblies)
            {
                plugins.Add(GetPluginDetailsFromAssembly(plugin));
            }

            return plugins;
        }

        public IPluginDetails GetPluginDetailsFromAssembly(string assemblyPath)
        {
            Assembly loadPlugin = Assembly.LoadFrom(assemblyPath);

            string assemblyName = loadPlugin.GetName().Name;

            Type toLoad = loadPlugin.GetType(assemblyName + ".PluginDetails");

            IPluginDetails details = Activator.CreateInstance(toLoad) as IPluginDetails;

            return details;
        }
    }
}