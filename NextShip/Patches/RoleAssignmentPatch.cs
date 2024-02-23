using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using NextShip.Api.Attributes;
using NextShip.Manager;
using UnityEngine;

namespace NextShip.Patches;

[Harmony]
internal class GameOptionsDataPatch
{
    [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.GetAdjustedNumImpostors))]
    [HarmonyPostfix]
    public static void GetAdjustedNumImpostorsPatch_Postfix(ref int __result)
    {
        __result = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.NumImpostors, 1, 3);
    }

    [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.Validate))]
    [HarmonyPostfix]
    public static void ValidatePatch_Postfix(GameOptionsData __instance)
    {
        __instance.NumImpostors = GameOptionsManager.Instance.CurrentGameOptions.NumImpostors;
    }
}

[Harmony]
internal class RoleManagerPatch
{
    [ServiceAdd] private static readonly NextRoleManager _nextRoleManager;

    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public static void SelectRoles_Postfix(RoleManager __instance)
    {
        GetPlayerRoleS(out var C, out var I);
        var CAssign = new Queue<PlayerControl>(C);
        var IAssign = new Queue<PlayerControl>(I);
        var _creator = _nextRoleManager.GetCreator();
        _StartAssign:
        var player = (CAssign.Count > 0 ? CAssign : IAssign).Dequeue();
        var role = _creator.GetAssign();
        _nextRoleManager.AssignRole(player, role);
        if (IAssign.Count > 0)
            goto _StartAssign;
    }

    private static void GetPlayerRoleS(out PlayerControl[] C, out PlayerControl[] I)
    {
        var ListC = CachedPlayer.AllPlayers.Where(n => n?.Data.Role.Role == RoleTypes.Crewmate)
            .Select(n => n.PlayerControl);
        var ListI = CachedPlayer.AllPlayers.Where(n => n?.Data.Role.Role == RoleTypes.Impostor)
            .Select(n => n.PlayerControl);
        C = ListC.ToArray();
        I = ListI.ToArray();
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.EndGame))]
    public static void OnGameEnd_Postfix(GameManager __instance)
    {
        NextPlayerManager.Instance.Clear();
        _nextRoleManager.Clear();
    }
}