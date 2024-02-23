using System;
using System.Reflection;
using HarmonyLib;
using NextShip.Api.Attributes;

namespace NextShip.Manager;

public static class RegisterManager
{
    public static void Registration()
    {
        Info("Register: Start Registration(开始注册)", filename: "RegisterManager");
        Assembly.GetExecutingAssembly().GetTypes().Do(StartRegistrations);
    }

    private static void StartRegistrations(Type type)
    {
        Il2CppRegisterAttribute.Registration(type);
        LoadAttribute.Registration(type);
        OptionLoad.Registration(type);
        TranslateTag.Registration(type);
    }
}