using HarmonyLib;

namespace NextShip.Patches;

[Harmony]
internal class ShowRolePatch
{
    
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    [HarmonyPrefix]
    public static void OnShouRolePatch(IntroCutscene __instance)
    {
        
    }
    
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowTeam))]
    [HarmonyPrefix]
    public static void OnShouTeamPatch(IntroCutscene __instance)
    {
        
    }
}