using System;
using System.IO;
using AmongUs.Data;
using AmongUs.Data.Legacy;
using AmongUs.Data.Player;
using HarmonyLib;

namespace NextShip.Patches;

// https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/SaveManagerPatch.cs
[HarmonyPatch]
public class SaveManagerPatch
{
    private static readonly string AUDataPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "LOW", "Innersloth",
            "Among Us");

    private static readonly bool SaveToTISInfo = false;

    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.FileName), MethodType.Getter)]
    [HarmonyPostfix]
    public static void FileNamePatch_Postfix(ref string __result)
    {
        if (!SaveToTISInfo) return;
        __result += "_TIS";
    }

    [HarmonyPatch(typeof(LegacySaveManager), nameof(LegacySaveManager.GetPrefsName))]
    [HarmonyPostfix]
    public static void LegacySaveManagerPatch_Postfix(ref string __result)
    {
        if (!SaveToTISInfo) return;
        __result += "_TIS";
    }

    [HarmonyPatch(typeof(AbstractSaveData), nameof(AbstractSaveData.TrySaveToJsonFile))]
    [HarmonyPostfix]
    public static void OnJsonSaveDPatch([HarmonyArgument(1)] string filename)
    {
        var path = NextPaths.TIS_ConfigPath + $"/{filename}";
        var sourcePath = AUDataPath.CombinePath(filename);
        Info($"name {filename} path1 {sourcePath}, path2 {path}");
        if (!File.Exists(path)) File.Create(path);
        File.Copy(sourcePath, path, true);
    }

    [HarmonyPatch(typeof(AbstractUserSaveData), nameof(AbstractUserSaveData.HandleLoad))]
    [HarmonyPrefix]
    public static void OnJsonLoadPatch(AbstractUserSaveData __instance)
    {
        var filename = __instance.GetFileName();
        var sourcePath = NextPaths.TIS_ConfigPath + $"/{filename}";
        var DestPath = AUDataPath.CombinePath(filename);
        if (!File.Exists(sourcePath) || !File.Exists(DestPath)) return;
        Info($"name {filename} path1 {sourcePath}, path2 {DestPath}");

        File.Copy(sourcePath, DestPath, true);
    }
}