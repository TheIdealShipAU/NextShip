﻿using System.Linq;
using HarmonyLib;

namespace NextShip.Patches;

[HarmonyPatch]
public static class ServerPath
{
    public static bool CurrentVanillaServer => Main.serverManager.CurrentRegion.IsVanilla();

    public static void autoAddServer()
    {
        IRegionInfo[] regionInfos =
        {
            createHttp("au-sh.pafyx.top", "梦服上海(新)", 22000, false),
            createHttp("au-as.duikbo.at", "Modded Asia (MAS)", 443, true),
            createHttp("www.aumods.xyz", "Modded NA (MNA)", 443, true),
            createHttp("au-eu.duikbo.at", "Modded EU (MEU)", 443, true)
        };

        if (!Main.isChinese) regionInfos.ToList().RemoveAt(0);

        foreach (var r in regionInfos)
        {
            if (Main.serverManager.AvailableRegions.Contains(r)) continue;
            Main.serverManager.AddOrUpdateRegion(r);
        }

        if (Main.IsDev) Main.serverManager.SetRegion(regionInfos[0]);
    }

    public static IRegionInfo createHttp(string ip, string name, ushort port, bool ishttps)
    {
        var serverIp = ishttps ? "https://" : "http://" + ip;
        var serverInfo = new ServerInfo(name, serverIp, port, false);
        ServerInfo[] ServerInfo = { serverInfo };
        return new StaticHttpRegionInfo(name, StringNames.NoTranslation, ip, ServerInfo).CastFast<IRegionInfo>();
    }

    [HarmonyPatch(typeof(Constants), nameof(Constants.GetBroadcastVersion))]
    [HarmonyPrefix]
    public static bool ConstantsGetBroadcastVersionPatch(ref int __result)
    {
        if (!CurrentVanillaServer) return true;

        __result = Constants.GetVersion(2222, 0, 0, 0);
        return false;
    }

    private static bool IsVanilla(this IRegionInfo regionInfo)
    {
        return regionInfo.TranslateName is StringNames.ServerAS or StringNames.ServerEU or StringNames.ServerNA
            or StringNames.ServerSA;
    }
}