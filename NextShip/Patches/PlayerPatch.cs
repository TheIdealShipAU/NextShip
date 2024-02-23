using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using InnerNet;
using JetBrains.Annotations;
using NextShip.Api.Enums;
using NextShip.Api.RPCs;
using NextShip.Manager;
using OtherAttribute;

namespace NextShip.Patches;

[Harmony]
public static class PlayerPatch
{
    public static readonly List<PlayerVersionInfo> AllPlayerVersionInfos = [];

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Awake))]
    [HarmonyPostfix]
    public static void PlayerControlAwake_PostfixPatch(PlayerControl __instance)
    {
        if (NextPlayerManager.Instance.TryGetPlayer(__instance, out _))
            return;
        NextPlayerManager.Instance.InitPlayer(__instance);
    }

    [HarmonyPatch(typeof(GameData), nameof(GameData.AddPlayer))]
    [HarmonyPostfix]
    public static void GameDataAddPlayer_Postfix([HarmonyArgument(0)] PlayerControl pc)
    {
        NextPlayerManager.Instance.CreateOrGetSetPlayerInfo(pc);
    }

    [HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.GetOrCreateClient))]
    [HarmonyPostfix]
    public static void GetOrCreateClient_Postfix(ClientData __result)
    {
        NextPlayerManager.Instance.CreateOrGetSetPlayerInfo(__result);
    }

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
    [HarmonyPostfix]
    public static void OnPlayerJoined(AmongUsClient __instance)
    {
        var writer = FastRpcWriter.StartNew();
        writer.SetRpcCallId((byte)SystemRPCFlag.VersionShare);
        writer.SetTargetObjectId(PlayerControl.LocalPlayer.NetId);
        writer.SetSendOption(SendOption.Reliable);
        writer.StartSendAllRPCWriter();
        writer.Write(PlayerControl.LocalPlayer.PlayerId);
        Main.Version.Write(writer);
        writer.Write(Main.BepInExVersion);
        
        if (!AllPlayerVersionInfos.Exists(n => n.Player == CachedPlayer.LocalPlayer.PlayerControl))
            AllPlayerVersionInfos.Add(new PlayerVersionInfo(CachedPlayer.LocalPlayer.PlayerControl, Main.Version, Main.BepInExVersion));
    }

    [FastReadAdd((byte)SystemRPCFlag.VersionShare)]
    public static void OnVersionShare(MessageReader reader)
    {
        var player = PlayerUtils.GetPlayerForId(reader.ReadByte());
        var version = new ShipVersion().Read(reader);
        var BepInExVersion = reader.ReadString();
        AllPlayerVersionInfos.Add(new PlayerVersionInfo(player, version, BepInExVersion));
    }
}

public record PlayerVersionInfo(PlayerControl Player, ShipVersion Version, string BepInExVersion);