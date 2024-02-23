using HarmonyLib;

namespace NextShip.Api;

internal static class APIHarmony
{
    static APIHarmony()
    {
        _Harmony = new Harmony("net.NextShip.Api");
    }

    internal static Harmony _Harmony { get; set; }
}