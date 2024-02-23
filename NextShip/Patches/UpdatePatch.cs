using HarmonyLib;
using InnerNet;
using UnityEngine;

namespace NextShip.Patches;

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
internal class HudManagerUpdatePatch
{
    private static void setPlayerNameColor(PlayerControl p, Color color)
    {
        p.cosmetics.nameText.color = color;
        if (MeetingHud.Instance == null) return;
        foreach (var player in MeetingHud.Instance.playerStates)
            if (player.NameText != null && p.PlayerId == player.TargetPlayerId)
                player.NameText.color = color;
    }

    private static void SetNameColors()
    {
        var localPlayer = CachedPlayer.LocalPlayer.PlayerControl;

    }

    private static void updateVentButton(HudManager __instance)
    {
        if (MeetingHud.Instance || CachedPlayer.LocalPlayer.Data.IsDead) __instance.ImpostorVentButton.Hide();
        else if (CachedPlayer.LocalPlayer.PlayerControl.Data.Role.IsImpostor &&
                 !__instance.ImpostorVentButton.isActiveAndEnabled) __instance.ImpostorVentButton.Show();
    }

    private static void updateUseButton(HudManager __instance)
    {
        if (MeetingHud.Instance) __instance.UseButton.Hide();
    }

    private static void updateSabotageButton(HudManager __instance)
    {
        if (MeetingHud.Instance) __instance.SabotageButton.Hide();
    }

    private static void updateMapButton(HudManager __instance)
    {
        if (__instance == null || __instance.MapButton.HeldButtonSprite == null) return;
        __instance.MapButton.HeldButtonSprite.color = Color.white;
    }

    private static void Postfix(HudManager __instance)
    {
        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started) return;
        SetNameColors();
        updateVentButton(__instance);
        updateSabotageButton(__instance);
        updateUseButton(__instance);
        updateMapButton(__instance);
    }
}