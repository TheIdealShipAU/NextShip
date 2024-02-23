#nullable enable
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;

namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class Il2CppRegisterAttribute(params Type[] interfaces) : Attribute
{
    public Il2CppRegisterAttribute() : this(Type.EmptyTypes)
    {
    }


    public Type[] Interfaces { get; } = interfaces;

    public static void Registration(Type? type)
    {
        Info("Start Registration", "Il2CppRegister");

        var attribute =
            type?.GetCustomAttribute<Il2CppRegisterAttribute>();

        if (attribute != null) registrationForTarget(type, attribute.Interfaces);

        Info("Complete Registration", "Il2CppRegister");
    }

    private static void registrationForTarget(Type? targetType, Type[] interfaces)
    {
        var targetBase = targetType?.BaseType;

        Il2CppRegisterAttribute? baseAttribute = null;

        if (targetBase != null)
            baseAttribute = targetBase.GetCustomAttribute<Il2CppRegisterAttribute>();

        if (baseAttribute != null)
            registrationForTarget(targetBase, baseAttribute.Interfaces);

        Debug($"Registration {targetType}", "Register");

        if (ClassInjector.IsTypeRegisteredInIl2Cpp(targetType)) return;

        try
        {
            ClassInjector.RegisterTypeInIl2Cpp
            (
                targetType,
                new RegisterTypeOptions
                {
                    Interfaces = interfaces
                }
            );
        }
        catch (Exception e)
        {
            var excStr = targetType.FullDescription();
            Error($"Registion Fail!!    Target:{excStr}\n Il2CppError:{e}", "Register");
        }
    }
}