using HarmonyLib;

namespace NextShip.Cosmetics.Patches;

[HarmonyPatch(typeof(HatManager))]
public static class HatManagerPatch
{
    private static bool initialized = true;

    [HarmonyPatch(nameof(HatManager.Initialize))]
    [HarmonyPostfix]
    public static void InitHatCache(HatManager __instance)
    {
        if (initialized) return;

        TaskUtils.StartTask(() => initialized = AllCosmeticsCache.StartCache(__instance));
    }
}