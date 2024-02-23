#nullable enable
using System.Collections;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace NextShip.Api.Utilities;

// from TheOtherRole
public class CachedPlayer
{
    public static readonly Dictionary<IntPtr, CachedPlayer> PlayerIntPtrS = new();
    public static readonly List<CachedPlayer> AllPlayers = [];
    public static CachedPlayer LocalPlayer = null!;
    public GameData.PlayerInfo Data = null!;
    public CustomNetworkTransform NetTransform = null!;
    public PlayerControl PlayerControl = null!;
    public byte PlayerId;
    public PlayerPhysics PlayerPhysics = null!;

    public Transform transform = null!;

    public static implicit operator bool(CachedPlayer? player)
    {
        return player != null && player.PlayerControl;
    }

    public static implicit operator PlayerControl(CachedPlayer player)
    {
        return player.PlayerControl;
    }

    public static implicit operator PlayerPhysics(CachedPlayer player)
    {
        return player.PlayerPhysics;
    }
}

[HarmonyPatch]
public static class CachedPlayerPatches
{
    public static bool CanAssign(this CachedPlayer? player)
    {
        return player is { Data: not null };
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Awake))]
    [HarmonyPostfix]
    public static void CachePlayerPatch(PlayerControl __instance)
    {
        if (__instance.notRealPlayer) return;
        var player = new CachedPlayer
        {
            transform = __instance.transform,
            PlayerControl = __instance,
            PlayerPhysics = __instance.MyPhysics,
            NetTransform = __instance.NetTransform
        };
        CachedPlayer.AllPlayers.Add(player);
        CachedPlayer.PlayerIntPtrS[__instance.Pointer] = player;

#if DEBUG
        foreach (var cachedPlayer in CachedPlayer.AllPlayers.Where(cachedPlayer => !cachedPlayer?.PlayerControl ||
                     !cachedPlayer.PlayerPhysics || !cachedPlayer.NetTransform ||
                     !cachedPlayer.transform))
            Error($"CachedPlayer {cachedPlayer?.PlayerControl.name} has null fields");
#endif
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnDestroy))]
    [HarmonyPostfix]
    public static void RemoveCachedPlayerPatch(PlayerControl __instance)
    {
        if (__instance.notRealPlayer) return;
        CachedPlayer.AllPlayers.RemoveAll(p => p!.PlayerControl.Pointer == __instance.Pointer);
        CachedPlayer.PlayerIntPtrS.Remove(__instance.Pointer);
    }

    [HarmonyPatch(typeof(GameData), nameof(GameData.Deserialize))]
    [HarmonyPostfix]
    public static void AddCachedDataOnDeserialize()
    {
        foreach (var cachedPlayer in CachedPlayer.AllPlayers)
        {
            cachedPlayer.Data = cachedPlayer.PlayerControl.Data;
            cachedPlayer.PlayerId = cachedPlayer.PlayerControl.PlayerId;
        }
    }

    [HarmonyPatch(typeof(GameData), nameof(GameData.AddPlayer))]
    [HarmonyPostfix]
    public static void AddCachedDataOnAddPlayer()
    {
        foreach (var cachedPlayer in CachedPlayer.AllPlayers)
        {
            cachedPlayer!.Data = cachedPlayer.PlayerControl.Data;
            cachedPlayer.PlayerId = cachedPlayer.PlayerControl.PlayerId;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Deserialize))]
    [HarmonyPostfix]
    public static void SetCachedPlayerId(PlayerControl __instance)
    {
        CachedPlayer.PlayerIntPtrS[__instance.Pointer]!.PlayerId = __instance.PlayerId;
    }

    [HarmonyPatch]
    private class CacheLocalPlayerPatch
    {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            var type = typeof(PlayerControl).GetNestedTypes(AccessTools.all)
                .FirstOrDefault(t => t.Name.Contains("Start"));
            return AccessTools.Method(type, nameof(IEnumerator.MoveNext));
        }

        [HarmonyPostfix]
        public static void SetLocalPlayer()
        {
            var localPlayer = PlayerControl.LocalPlayer;
            if (!localPlayer)
            {
                CachedPlayer.LocalPlayer = null!;
                return;
            }

            var cached = CachedPlayer.AllPlayers.FirstOrDefault(p => p!.PlayerControl.Pointer == localPlayer.Pointer);
            if (cached != null) CachedPlayer.LocalPlayer = cached;
        }
    }
}