using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NextShip.Api.Attributes;
using NextShip.Api.Bases;

namespace NextShip.DIY.Plugins;

public class PluginLoadService
{
    private readonly List<PluginLoadInfo> _pluginLoadInfos;
    private readonly PluginManager _pluginManager;
    private readonly IServiceProvider _serviceProvider;

    public PluginLoadService(PluginManager manager, IServiceProvider serviceProvider)
    {
        _pluginLoadInfos = manager.PluginLoadInfos;
        _pluginManager = manager;
        _serviceProvider = serviceProvider;
        Load();
    }

    private void Load()
    {
        Info("开始加载插件");
        foreach (var plugin in _pluginLoadInfos)
        {
            plugin.Plugin = (ShipPlugin)ActivatorUtilities.CreateInstance(_serviceProvider, plugin._Type);
            plugin.PluginInfo = plugin._Type.GetCustomAttribute<ShipPluginInfo>();
            plugin.Load();

            ServiceAddAttribute.Registration(_serviceProvider, plugin._Assembly);
            _pluginManager.Plugins.Add(plugin.Plugin);
            Info($"加载插件 {plugin._Assembly.GetName().Name} {plugin._Type.Name} {plugin._path}");
        }

        _pluginManager.PluginLoadInfos = _pluginLoadInfos;
    }
}