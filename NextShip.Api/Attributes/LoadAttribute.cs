using System.Collections;
using System.Reflection;

namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor)]
public sealed class LoadAttribute(LoadMode mode = LoadMode.Load) : Attribute
{
    public static readonly List<LoadAttribute> Loads = [];

    public static string[] MethodNames = EnumHelper.GetAllNames<LoadMode>();

    public IEnumerator Enumerator;
    public LoadMode Mode = mode;

    public static void Registration(Type type)
    {
        Info("Start Registration", filename: MethodUtils.GetClassName());

        if (type.GetCustomAttribute<LoadAttribute>() == null) return;

        ConstructorInfo constructor;
        if (
            (
                constructor = type.GetConstructor
                (
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.NonPublic,
                    Array.Empty<Type>()
                )
            )
            != null && constructor.Is<LoadAttribute>())
            constructor.Invoke(null, null);

        foreach (var MethodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (MethodInfo.ReturnType != typeof(IEnumerator))
                continue;

            var load = MethodInfo.GetCustomAttribute<LoadAttribute>();
            LoadMode? mode = null;
            if (Enum.TryParse(MethodInfo.Name, out LoadMode OutMode))
                mode = OutMode;

            if (load != null)
            {
                load.Enumerator = MethodInfo.Invoke(null, null) as IEnumerator;
                Loads.Add(load);
                continue;
            }

            if (mode != null)
                Loads.Add(new LoadAttribute
                {
                    Mode = (LoadMode)mode,
                    Enumerator = MethodInfo.Invoke(null, null) as IEnumerator
                });
        }

        Info($"Statically Initialized Class {type}", "LoadAttribute");
    }
}