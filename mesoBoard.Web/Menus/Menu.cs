using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Web.Areas.Admin.Models;
using mesoBoard.Framework.Core;

namespace mesoBoard.Web
{
    public static partial class Menus
    {
        public static IList<PluginMenu> PluginsAdminCP()
        {
            List<PluginMenu> menus = new List<PluginMenu>();
            var repo = ServiceLocator.Get<IRepository<Plugin>>();
            string[] pluginFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"), "*.dll");

            foreach (string plugin in pluginFiles)
            {
                Assembly loadPlugin = Assembly.LoadFrom(plugin);
                string name = loadPlugin.GetName().Name;
                Type toLoad = loadPlugin.GetType(name + ".PluginDetails");
                IPluginDetails pluginDetails = Activator.CreateInstance(toLoad) as IPluginDetails;

                if (repo.First(item => item.Name.Equals(pluginDetails.Name)) == null)
                    continue;

                menus.Add(new PluginMenu
                {
                    MainLink = pluginDetails.AdminMenu.ParentLink,
                    ChildLinks = pluginDetails.AdminMenu.ChildLinks
                });
            }

            return menus;
        }
    }
}