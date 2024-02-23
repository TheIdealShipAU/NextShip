#nullable enable
namespace NextShip.Api.Config;

/*
 * form https://github.com/NuclearPowered/Reactor/blob/master/Reactor.Networking.Shared/Mod.cs
 */
public class Mod(string id, string version, ModFlags flags, string? name)
{
    /// <summary>
    ///     Gets the id of the mod.
    /// </summary>
    public string Id { get; } = id;

    /// <summary>
    ///     Gets the version of the mod.
    /// </summary>
    public string Version { get; } = version;

    /// <summary>
    ///     Gets the flags of the mod.
    /// </summary>
    public ModFlags Flags { get; } = flags;

    /// <summary>
    ///     Gets the name of the mod.
    /// </summary>
    public string? Name { get; } = name;
}