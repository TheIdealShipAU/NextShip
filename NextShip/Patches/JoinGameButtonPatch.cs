using System.Text.RegularExpressions;
using HarmonyLib;
using UnityEngine;

namespace NextShip.Patches;

[HarmonyPatch(typeof(JoinGameButton), nameof(JoinGameButton.OnClick))]
internal partial class JoinGameButtonPatch
{
    public static void Prefix(JoinGameButton __instance)
    {
        if (__instance.GameIdText == null) return;
        if (__instance.GameIdText.text == "" &&
            MyRegex().IsMatch(GUIUtility.systemCopyBuffer.Trim('\r', '\n')))
            __instance.GameIdText.SetText(GUIUtility.systemCopyBuffer.Trim('\r', '\n'));
    }

    [GeneratedRegex(@"^[a-zA-Z]{6}$")]
    private static partial Regex MyRegex();
}