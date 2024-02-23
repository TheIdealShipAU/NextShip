namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShipPluginInfo(string Id, ShipVersion Version, string Name) : Attribute
{
    public string Id { get; } = Id;
    public ShipVersion Version { get; } = Version;
    public string Name { get; } = Name;
}