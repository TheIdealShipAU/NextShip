namespace NextShip.Api.RPCs;

public static class FastRPCExtension
{
    public static void UseFastRPC()
    {
        _Harmony.PatchAll(typeof(FastRpcReaderPatch));
    }
}