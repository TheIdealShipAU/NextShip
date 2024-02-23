namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class PluginCompatibility(
    string pluginName,
    CompatibilityLevel compatibility = CompatibilityLevel.Compatible)
    : Attribute
{
    public CompatibilityLevel Compatibility = compatibility;
    public string PluginName = pluginName;
}