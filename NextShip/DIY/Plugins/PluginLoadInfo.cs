using System;
using System.Reflection;
using NextShip.Api.Attributes;
using NextShip.Api.Bases;

namespace NextShip.DIY.Plugins;

internal class PluginLoadInfo
{
    public string _path { get; set; }
    public Assembly _Assembly { get; set; }
    public Type _Type { get; set; }
    public ShipPlugin Plugin { get; set; }

    public ShipPluginInfo PluginInfo { get; set; }

    internal void Load()
    {
        Plugin.Load();
    }
}