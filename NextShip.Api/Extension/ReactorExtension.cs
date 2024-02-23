using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Hazel;
using InnerNet;
using NextShip.Api.Config;
using NextShip.Api.Patches;

namespace NextShip.Api.Extension;

/*
 * form https://github.com/NuclearPowered/Reactor
 */

[Harmony]
public static class ReactorExtension
{
    public static readonly HashSet<Mod> HashSet_Mods = new();

    public static readonly Dictionary<int, HashSet<Mod>> All_Mod = new();

    public static bool ReactorHandshake { get; private set; }

    public static void UseReactorHandshake()
    {
        ReactorHandshake = true;
        UseModList();
        _Harmony.PatchAll(typeof(InnerNetClientPatch));
        _Harmony.PatchAll(typeof(ReactorExtension));
    }

    public static void DisableReactorHandshake()
    {
        ReactorHandshake = false;
    }

    public static void GetHandshakeInfo()
    {
    }

    public static void MessageWrite(ref MessageWriter handshakeWriter)
    {
        handshakeWriter.Write(((ulong)0x72656163746f72 << 8) | 2);

        // 写入握手模组信息
        handshakeWriter.WritePacked(HashSet_Mods.Count);
        foreach (var varMod in HashSet_Mods)
        {
            handshakeWriter.Write(varMod.Id);
            handshakeWriter.Write(varMod.Version);
            handshakeWriter.Write((ushort)varMod.Flags);
            if (varMod.Flags.HasFlag(ModFlags.RequireOnAllClients))
                handshakeWriter.Write(varMod.Name);
        }
    }

    private static void OnPluginLoad(PluginInfo pluginInfo, BasePlugin plugin)
    {
        var pluginType = plugin.GetType();

        var mod = new Mod(
            pluginInfo.Metadata.GUID,
            pluginInfo.Metadata.Version.Clean(),
            ReactorModFlagsAttribute.GetModFlags(pluginType),
            pluginInfo.Metadata.Name
        );

        HashSet_Mods.Add(mod);
    }

    private static void UseModList()
    {
        foreach (var existingPlugin in IL2CPPChainloader.Instance.Plugins.Values.Where(existingPlugin =>
                     existingPlugin.Instance != null))
            OnPluginLoad(existingPlugin, (BasePlugin)existingPlugin.Instance);

        IL2CPPChainloader.Instance.PluginLoad += (pluginInfo, _, plugin) => OnPluginLoad(pluginInfo, plugin);
    }

    [HarmonyPatch(typeof(InnerNetClient._HandleGameDataInner_d__39),
        nameof(InnerNetClient._HandleGameDataInner_d__39.MoveNext))]
    [HarmonyPrefix]
    public static bool _HandleGameDataInner_d__39_Prefix(InnerNetClient._HandleGameDataInner_d__39 __instance,
        ref bool __result)
    {
        if (!ReactorHandshake) return true;

        var innerNetClient = __instance.__4__this;
        var reader = __instance.reader;

        if (__instance.__1__state != 0 || reader.Tag != InnerNetClient.SceneChangeFlag) return true;

        var clientId = reader.ReadPackedInt32();
        var clientData = innerNetClient.FindClientById(clientId);
        var sceneName = reader.ReadString();

        if (clientData == null || string.IsNullOrWhiteSpace(sceneName)) return true;

        if (reader.BytesRemaining >= sizeof(ulong) && Read(reader))
        {
            var modCount = reader.ReadPackedInt32();
            var mods = new HashSet<Mod>(modCount);

            for (var i = 0; i < modCount; i++)
            {
                var id = reader.ReadString();
                var version = reader.ReadString();
                var flags = (ModFlags)reader.ReadUInt16();
                var name = (flags & ModFlags.RequireOnAllClients) != 0 ? reader.ReadString() : null;

                mods.Add(new Mod(id, version, flags, name));
            }

            All_Mod[clientId] = mods;
        }

        if (innerNetClient.AmHost)
            innerNetClient.StartCoroutine(innerNetClient.CoOnPlayerChangedScene(clientData, sceneName));
        else
            reader.Recycle();

        __result = false;
        return false;

        bool Read(MessageReader _reader)
        {
            var value = _reader.ReadUInt64();
            var magic = value >> 8;
            var version = (byte)(value & 0xFF);
            return magic == 0x72656163746f72 && version == 2;
        }
    }

    [HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.DisconnectInternal))]
    [HarmonyPatch(typeof(EndGameResult), nameof(EndGameResult.Create))]
    [HarmonyPostfix]
    public static void Clear_Postfix()
    {
        All_Mod.Clear();
    }
}