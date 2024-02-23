using HarmonyLib;

namespace NextShip.Api.Utils;

public static class ServerUtils
{
    public static void AddServer(IRegionInfo region) =>
        ServerManager.Instance.AddOrUpdateRegion(region);

    public static void AddServers(IEnumerable<IRegionInfo> regionInfos) =>
        regionInfos.Do(ServerManager.Instance.AddOrUpdateRegion);
    
    public static List<IRegionInfo> CreateHttpInfos(params (string ip, string name, ushort port, bool isHttps)[] Infos)
    {
        return Infos.Select(tuple => createHttpInfo(tuple.ip, tuple.name, tuple.port, tuple.isHttps)).ToList();
    }
    
    public static IRegionInfo createHttpInfo(string ip, string name, ushort port, bool isHttps = false)
    {
        var serverIp = isHttps ? "https://" : "http://" + ip;
        var serverInfo = new ServerInfo(name, serverIp, port, false);
        ServerInfo[] ServerInfo = [serverInfo];
        return new StaticHttpRegionInfo(name, StringNames.NoTranslation, ip, ServerInfo).CastFast<IRegionInfo>();
    }
    
    public static bool IsVanilla(this IRegionInfo regionInfo)
    {
        return regionInfo.TranslateName is StringNames.ServerAS or StringNames.ServerEU or StringNames.ServerNA
            or StringNames.ServerSA;
    }
}