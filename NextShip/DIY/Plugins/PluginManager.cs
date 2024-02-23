using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using NextShip.Api.Attributes;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;
using NextShip.Services;

namespace NextShip.DIY.Plugins;

public class PluginManager : Manager<PluginManager>
{
    private readonly List<(Assembly, Type, ShipPlugin)> PluginCreateS = [];
    internal List<PluginLoadInfo> PluginLoadInfos = [];
    private readonly List<string> PluginPathS = [];

    public readonly List<ShipPlugin> Plugins = [];


    public void InitPlugins()
    {
        FindPlugins();
        LoadAssemblyFormPath();
    }

    public void OnServiceBuild(ServiceBuilder serviceBuilder)
    {
        foreach (var plugin in PluginLoadInfos)
            INextAdd.GetAdds(plugin._Assembly).Do(n => n.ServiceAdd(serviceBuilder));

        serviceBuilder._collection.AddActivatedSingleton(provider =>
            ActivatorUtilities.CreateInstance<PluginLoadService>(provider, this));
    }

    private void Load(string path)
    {
        var fileName = Path.GetFileName(path);
        if (!path.Contains(".dll"))
        {
            Info($"{fileName} 疑似不是dll", filename: MethodUtils.GetClassName());
            return;
        }

        var assembly = Assembly.LoadFile(path);
        var types = assembly.GetTypes().Where(n => n.BaseType == typeof(ShipPlugin));
        var has = false;
        var IsInherit = false;
        types.Do
        (n =>
            {
                IsInherit = true;
                has = true;

                var plugin = (ShipPlugin)ActivatorUtilities.CreateInstance(Main._Service._Provider, n);

                var shipPluginInfo = n.GetCustomAttribute<ShipPluginInfo>();
                if (shipPluginInfo != null)
                    plugin.ShipPluginInfo = shipPluginInfo;

                var Compatibilities = n.GetCustomAttributes<PluginCompatibility>();
                plugin.PluginCompatibilities = Compatibilities.ToList();

                PluginCreateS.Add((assembly, n, plugin));
            }
        );
        if (!has) Info($"{fileName} 未注册插件", filename: MethodUtils.GetClassName());

        if (!IsInherit) Info($"{fileName} no Inherit", filename: MethodUtils.GetClassName());
    }

    private static void Load((Assembly, Type, ShipPlugin) pluginTuple)
    {
        var shipPluginInfo = pluginTuple.Item3.ShipPluginInfo;
        try
        {
            pluginTuple.Item3.Load();
            Main.Adds.AddRange(pluginTuple.Item3.NextAdd());
            Info($"Name:{shipPluginInfo.Name} . Version:{shipPluginInfo.Version} . Id:{shipPluginInfo.Id} 运行成功 ",
                filename: MethodUtils.GetClassName());
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }

    private void FindPlugins()
    {
        var matcher = new Matcher();
        matcher.AddInclude(".dll");
        PluginPathS.AddRange(matcher.GetResultsInFullPath(NextPaths.TIS_PluginsPath).ToList());
    }

    private void LoadAssemblyFormPath()
    {
        foreach (var varPath in PluginPathS)
        {
            var assembly = Assembly.LoadFile(varPath);
            var type = assembly.GetTypes().FirstOrDefault(n => n.BaseType == typeof(ShipPlugin));
            if (type == null) continue;
            var LoadInfo = new PluginLoadInfo
            {
                _path = varPath,
                _Assembly = assembly,
                _Type = type
            };
            Info($"初始化插件 path:{varPath} type:{type}");
        }
    }
}