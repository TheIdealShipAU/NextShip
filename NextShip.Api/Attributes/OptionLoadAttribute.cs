using System.Reflection;
using HarmonyLib;

namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OptionLoad : Attribute
{
    public static readonly List<MethodInfo> MethodInfos = [];

    public static void Registration(Type type)
    {
        Info("Start Registration", filename: MethodUtils.GetClassName());
        var ConstructorS = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
        ConstructorS.Do(Add);

        static void Add(MethodInfo methodInfo)
        {
            if (methodInfo.GetCustomAttribute<OptionLoad>() == null) return;
            MethodInfos.Add(methodInfo);
            Debug($"Add {methodInfo.Name}");
        }

        Info($"Statically Initialized Class {type}", filename: MethodUtils.GetClassName());
    }

    public static void StartOptionLoad()
    {
        MethodInfos.Do(n => n.Invoke(null, null));
    }
}