using System.Reflection;

namespace NextShip.Api.Attributes;

// form https://github.com/NuclearPowered/Reactor/blob/master/Reactor/Networking/Attributes/ReactorModFlagsAttribute.cs

/// <summary>
///     Describes the <see cref="ModFlags" /> of the annotated plugin class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ReactorModFlagsAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ReactorModFlagsAttribute" /> class.
    /// </summary>
    /// <param name="flags">Flags of the mod.</param>
    public ReactorModFlagsAttribute(ModFlags flags)
    {
        Flags = flags;
    }

    /// <summary>
    ///     Gets flags of the mod.
    /// </summary>
    public ModFlags Flags { get; }

    internal static ModFlags GetModFlags(Type type)
    {
        var attribute = type.GetCustomAttribute<ReactorModFlagsAttribute>();
        if (attribute != null) return attribute.Flags;

        var metadataAttribute = type.Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .SingleOrDefault(x => x.Key == "Reactor.ModFlags");
        return metadataAttribute is { Value: not null } ? Enum.Parse<ModFlags>(metadataAttribute.Value) : ModFlags.None;
    }
}