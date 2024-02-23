namespace NextShip.Api.Enums;

/*
 * form https://github.com/NuclearPowered/Reactor/blob/master/Reactor.Networking.Shared/ModFlags.cs
 */

/// <summary>
///     Represents flags of the mod.
///     模组使用兼容
/// </summary>
[Flags]
public enum ModFlags : ushort
{
    /// <summary>
    ///     No flags.
    ///     没有
    /// </summary>
    None = 0,

    /// <summary>
    ///     Requires all clients in a lobby to have the mod.
    ///     所有人都拥有
    /// </summary>
    RequireOnAllClients = 1 << 0,

    /// <summary>
    ///     Requires the server to have a plugin that handles the mod.
    ///     在服务器需要
    /// </summary>
    RequireOnServer = 1 << 1,

    /// <summary>
    ///     Requires the host of the lobby to have the mod.
    ///     仅房主需要
    /// </summary>
    RequireOnHost = 1 << 2
}