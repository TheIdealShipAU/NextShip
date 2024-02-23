using HarmonyLib;
using NextShip.Manager;

namespace NextShip.Patches;

[Harmony]
public static class PlayerPhysicsPatch
{
    // 运行后修改幽灵移速
    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
    [HarmonyPostfix]
    public static void Update_Postfix(PlayerPhysics __instance)
    {
        var playerInfo = NextPlayerManager.Instance.CreateOrGetSetPlayerInfo(__instance.myPlayer);
        __instance.GhostSpeed = playerInfo.GhostSpeed;
        __instance.Speed = playerInfo.BodySpeed;
    }
}