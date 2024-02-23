using System.Collections.Generic;
using HarmonyLib;
using NextShip.UI.Module;
using UnityEngine;

namespace NextShip.Options.Patches;

[HarmonyPatch]
public class OptionsConsolePatch
{
    public static Dictionary<int, bool> CanUseMenDictionary = new();

    public static bool AllowNoHostUse = false;
    public static bool IsNextMenu;
    public static bool EnablenNextOption;

    private static GameObject NextMenuParent;

    [HarmonyPatch(typeof(OptionsConsole), nameof(OptionsConsole.Use))]
    [HarmonyPrefix]
    public static bool OptionsConsoleUsePatch(OptionsConsole __instance)
    {
        /*if (!AllowNoHostUse) return true;*/

        var @object = PlayerControl.LocalPlayer;
        var couldUse = @object.CanMove;
        var canUse =
            couldUse
            && (!CanUseMenDictionary.TryGetValue(CachedPlayer.LocalPlayer.PlayerId, out var canUsed) || canUsed)
            && Vector2.Distance(@object.GetTruePosition(), __instance.transform.position) <= __instance.UsableDistance;

        if (!canUse) return false;


        var nextOptionMenu = NextOptionMenu.Instance;
        nextOptionMenu ??= new NextOptionMenu(__instance, NextMenuParent);
        nextOptionMenu.__instance = __instance;

        if (!nextOptionMenu.Initd) nextOptionMenu.Init();

        if (nextOptionMenu.Initd)
            OpenNextOptionMenu(__instance);
        else
            OpenVanillaOptionMenu(__instance);

        return false;
    }

    public static void OpenVanillaOptionMenu(OptionsConsole __instance)
    {
        if (!Camera.main) return;
        PlayerControl.LocalPlayer.NetTransform.Halt();
        var optionMenu = Object.Instantiate(__instance.MenuPrefab, Camera.main!.transform, false);
        optionMenu.transform.localPosition = __instance.CustomPosition;
        FastDestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, optionMenu.gameObject, null);
        IsNextMenu = false;
    }


    private static void OpenNextOptionMenu(OptionsConsole __instance)
    {
        if (!Camera.main) return;
        IsNextMenu = NextOptionMenu.Instance.OpenMenu(__instance.CustomPosition);
        if (!IsNextMenu) OpenVanillaOptionMenu(__instance);
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    [HarmonyPostfix]
    public static void OnHudManagerStart()
    {
        NextMenuParent = new GameObject("NextMenuParent");
    }
}