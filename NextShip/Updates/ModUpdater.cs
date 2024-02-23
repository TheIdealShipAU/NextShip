using System.Threading.Tasks;
using BepInEx.Configuration;

namespace NextShip.Updates;

public class ModUpdater
{
    // HUdate=HasUpdate 判断是否有更新
    public static bool HUpdate = VersionManager.HasUpdate;

    public static ConfigEntry<bool> AutoUpdate;

    // 模组下载链接
    public string ModDownloadURL;

    public Task UpdateMod()
    {
        return Task.CompletedTask;
    }
}