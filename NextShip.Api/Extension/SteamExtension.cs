using BepInEx;

namespace NextShip.Api.Extension;

public static class SteamExtension
{
    public const string file_Name = "steam_appid.txt";

    public const string Among_Us_SteamId = "945360";

    public static void UseSteamIdFile()
    {
        var path = Paths.GameRootPath.CombinePath(file_Name);
        if (!File.Exists(path))
            File.WriteAllText(path!, Among_Us_SteamId);
        else
        {
            return;
        }
        Info("Use steam_appid.txt");
    }
}