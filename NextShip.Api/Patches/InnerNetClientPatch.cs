using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using InnerNet;
using NextShip.Api.Extension;

namespace NextShip.Api.Patches;

[Harmony]
public static class InnerNetClientPatch
{
    public static Il2CppStructArray<byte> GetDataByte(Il2CppStructArray<byte> original)
    {
        var Writer = new MessageWriter(1000);
        Writer.Write(original);

        if (ReactorExtension.ReactorHandshake)
            ReactorExtension.MessageWrite(ref Writer);

        if (PasswordVerification.Enable)
            PasswordVerification.Write(ref Writer);

        var bytes = Writer.ToByteArray(true);
        Writer.Recycle();
        return bytes;
    }

    [HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.GetConnectionData))]
    [HarmonyPostfix]
    public static void GetConnectionData_Postfix(ref Il2CppStructArray<byte> __result)
    {
        __result = GetDataByte(__result);
    }
}