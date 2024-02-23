using System.Linq;
using HarmonyLib;

namespace NextShip.Patches;

[Harmony]
public static class GameStartManagerPatch
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    [HarmonyPostfix]
    public static void OnGameStartManagerUpdatePatch()
    {
        
    }
}