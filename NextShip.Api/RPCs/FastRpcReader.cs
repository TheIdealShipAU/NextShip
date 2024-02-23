using System.Reflection;
using HarmonyLib;
using Hazel;
using OtherAttribute;

namespace NextShip.Api.RPCs;

public class FastRpcReader
{
    public byte CallId;
    public Action<MessageReader> HandleRpc;
}

[Harmony]
public static class FastRpcReaderPatch
{
    public static readonly List<FastRpcReader> AllFastRpcReader = [];

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.HandleGameDataInner))]
    [HarmonyPrefix]
    public static void InnerNet_ReaderPath([HarmonyArgument(0)] MessageReader reader)
    {
        if (AllFastRpcReader.Count <= 0) return;
        var HandleReader = MessageReader.Get(reader);
        HandleReader.Position = 0;
        var tag = reader.Tag;
        if (tag != (int)DataFlags.Rpc)
            return;
        try
        {
            HandleReader.ReadPackedUInt32();
            var callId = HandleReader.ReadByte();
            AllFastRpcReader.Where(n => n.CallId == callId).Do(n => n.HandleRpc(MessageReader.Get(HandleReader)));
        }
        catch (Exception e)
        {
            Exception(e);
        }

        finally
        {
            reader.Recycle();
        }
    }

    public static void AddFormAssembly(Assembly assembly)
    {
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            var methods = type.GetMethods().Where(n => n.IsDefined(typeof(FastReadAdd)) && n.IsStatic).ToList();
            foreach (var method in methods)
            {
                var FastReadAdd = method.GetCustomAttribute<FastReadAdd>();
                if (FastReadAdd == null)
                    continue;

                if (method.GetGenericArguments()[0] == typeof(MessageReader))
                    AllFastRpcReader.Add(new FastRpcReader
                    {
                        CallId = FastReadAdd.CallId,
                        HandleRpc = n => method.Invoke(null, [n])
                    });
            }
        }
    }
}
