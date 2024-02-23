using JetBrains.Annotations;

namespace OtherAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor)]
[MeansImplicitUse]
public class FastAddRole : Attribute;