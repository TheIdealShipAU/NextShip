using HarmonyLib;

namespace NextShip.Cosmetics.Patches;

[HarmonyPatch]
public static class HatPatch
{
    [HarmonyPatch(typeof(HatParent), nameof(HatParent.SetHat), typeof(int))]
    [HarmonyPrefix]
    public static bool HatParent_SetHat_PrefixPatch(HatParent __instance, int color)
    {
        return !CustomCosmeticsManager.AllCustomCosmeticNameAndInfo.ContainsKey(__instance.Hat.name);
    }

    [HarmonyPatch(typeof(CosmeticsCache), nameof(CosmeticsCache.GetHat))]
    [HarmonyPrefix]
    public static bool Prefix(CosmeticsCache __instance, string id, ref HatViewData __result)
    {
        Info($"cache Get{id}");
        if (!(id.StartsWith("Mod_") || CustomCosmeticsManager.AllCosmeticId.Contains(id))) return true;
        __result = CustomCosmeticsManager.AllCustomHatViewData[id];
        return false;
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.HandleAnimation))]
    private static void PlayerPhysicsHandleAnimationPatch(PlayerPhysics __instance)
    {
        if (!CustomCosmeticsManager.AllCustomCosmeticNameAndInfo.ContainsKey(__instance.myPlayer.cosmetics.hat.Hat
                .name)) return;

        var currentAnimation = __instance.Animations.Animator.GetCurrentAnimation();
        if (currentAnimation == __instance.Animations.group.ClimbUpAnim ||
            currentAnimation == __instance.Animations.group.ClimbDownAnim) return;

        var hatParent = __instance.myPlayer.cosmetics.hat;
        if (hatParent == null || hatParent.Hat == null) return;

        if (!CustomCosmeticsManager.AllCustomCosmeticNameAndInfo.TryGetValue(hatParent.Hat.name, out var info)) return;
        if (info.FlipResource != null)
            hatParent.FrontLayer.sprite = __instance.FlipX
                ? CustomCosmeticsManager.GetSprite(info.FlipResource)
                : CustomCosmeticsManager.GetSprite(info.Resource);

        if (info.BackFlipResource == null) return;
        hatParent.BackLayer.sprite = __instance.FlipX
            ? CustomCosmeticsManager.GetSprite(info.BackFlipResource)
            : CustomCosmeticsManager.GetSprite(info.BackResource);
    }
}