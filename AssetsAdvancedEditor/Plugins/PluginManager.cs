﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AssetsAdvancedEditor.Assets;
using AssetsTools.NET.Extra;

namespace AssetsAdvancedEditor.Plugins
{
    public class PluginManager
    {
        private AssetsManager Am { get; }
        private List<PluginInfo> LoadedPlugins { get; }

        public PluginManager(AssetsManager am)
        {
            LoadedPlugins = new List<PluginInfo>();
            Am = am;
        }

        public bool LoadPlugin(string path)
        {
            var asm = Assembly.LoadFrom(path);
            foreach (var type in asm.GetTypes())
            {
                if (!typeof(IPlugin).IsAssignableFrom(type)) continue;
                var typeInst = Activator.CreateInstance(type);
                if (typeInst == null)
                    return false;

                var plugInst = (IPlugin)typeInst;
                var plugInf = plugInst.Init();
                LoadedPlugins.Add(plugInf);
                return true;
            }
            return false;
        }

        public void LoadPluginsInDirectory(string directory)
        {
            Directory.CreateDirectory(directory);
            foreach (var file in Directory.EnumerateFiles(directory, "*.dll"))
            {
                LoadPlugin(file);
            }
        }

        public List<PluginMenuInfo> GetSupportedPlugins(List<AssetContainer> selectedAssets)
        {
            var menuInfos = new List<PluginMenuInfo>();
            foreach (var pluginInf in LoadedPlugins)
            {
                foreach (var option in pluginInf.Options)
                {
                    var supported = option.IsValidForPlugin(Am, selectedAssets);
                    if (!supported) continue;
                    var menuInf = new PluginMenuInfo(pluginInf, option);
                    menuInfos.Add(menuInf);
                }
            }
            return menuInfos;
        }
    }
}
