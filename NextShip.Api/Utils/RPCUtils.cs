using Hazel;

namespace NextShip.Api.Utils;

public static class RPCUtils
{
    public static void Create(byte rpc, byte[] bytes = null, int[] ints = null, float[] floats = null,
        bool[] bools = null, string[] strings = null)
    {
        var rpcStart = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, rpc,
            SendOption.Reliable);
        if (bytes != null)
            foreach (var b in bytes)
                rpcStart.Write(b);

        if (ints != null)
            foreach (var i in ints)
                rpcStart.Write(i);

        if (floats != null)
            foreach (var f in floats)
                rpcStart.Write(f);

        if (bools != null)
            foreach (var bo in bools)
                rpcStart.Write(bo);

        if (strings != null)
            foreach (var str in strings)
                rpcStart.Write(str);

        AmongUsClient.Instance.FinishRpcImmediately(rpcStart);
    }

    public static void ReadValue(MessageReader reader)
    {
        for (var b = 0; b < ReadRPCValue.byteL; b++)
        {
            var ByteValue = reader.ReadByte();
            ReadRPCValue.bytes[b] = ByteValue;
        }

        for (var i = 0; i < ReadRPCValue.intL; i++)
        {
            var IntValue = reader.ReadInt32();
            ReadRPCValue.ints[i] = IntValue;
        }

        for (var bo = 0; bo < ReadRPCValue.boolL; bo++)
        {
            var BoolValue = reader.ReadBoolean();
            ReadRPCValue.bools[bo] = BoolValue;
        }

        for (var f = 0; f < ReadRPCValue.floatL; f++)
        {
            var FloatValue = reader.ReadSingle();
            ReadRPCValue.floats[f] = FloatValue;
        }

        for (var s = 0; s < ReadRPCValue.stringL; s++)
        {
            var StringValue = reader.ReadString();
            ReadRPCValue.strings[s] = StringValue;
        }
    }

    public static void ReadValueLength(MessageReader reader)
    {
        ReadRPCValue.ClearAll();
        for (var length = reader.ReadInt32(); length == 0; length--)
        {
            var type = reader.ReadByte();
            switch (type)
            {
                case (byte)RPCReadType.Byte:
                    ReadRPCValue.byteL = reader.ReadInt32();
                    break;

                case (byte)RPCReadType.Int:
                    ReadRPCValue.intL = reader.ReadInt32();
                    break;

                case (byte)RPCReadType.Bool:
                    ReadRPCValue.boolL = reader.ReadInt32();
                    break;

                case (byte)RPCReadType.Float:
                    ReadRPCValue.floatL = reader.ReadInt32();
                    break;

                case (byte)RPCReadType.String:
                    ReadRPCValue.stringL = reader.ReadInt32();
                    break;
            }
        }

        ReadValue(reader);
    }
}

public static class ReadRPCValue
{
    public static byte[] bytes;
    public static int[] ints;
    public static bool[] bools;
    public static float[] floats;
    public static string[] strings;
    public static int byteL;
    public static int intL;
    public static int boolL;
    public static int floatL;
    public static int stringL;

    public static void ClearAll()
    {
        bytes = null;
        ints = null;
        bools = null;
        floats = null;
        strings = null;
        byteL = 0;
        intL = 0;
        boolL = 0;
        floatL = 0;
        stringL = 0;
    }
}