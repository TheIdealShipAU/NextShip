#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using InnerNet;
using UnityEngine;

namespace NextShip.Manager;

public class InstanceManager
{
    public readonly Dictionary<Type, List<object?>?> Instances = new();

    public void RegisterInstance<T>(T instance)
    {
        if (instance != null) 
            RegisterInstance(typeof(T), instance);
        else
        {
            Error("实例添加失败为空");
        }
    }

    public void RegisterInstance(Type type, object obj) => (Instances[type] ??= []).Add(obj);

    public T? Get<T>() where T : class => Get(typeof(T)) as T;
    
    public List<T>? Gets<T>() where T : class => Gets(typeof(T)) as List<T>;

    public object? Get(Type type)
    {
        return Instances.FirstOrDefault(n => n.Key == type).Value?.FirstOrDefault();
    }
    
    public IEnumerable<object?>? Gets(Type type)
    {
        return Instances.FirstOrDefault(n => n.Key == type).Value;
    }
}

[HarmonyPatch]
internal static class InstanceManagerPatch
{
    private static IEnumerable<Type> InnerNetObjectTypes { get; } = typeof(InnerNetObject).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(MonoBehaviour))).ToList();

    public static IEnumerable<MethodBase> TargetMethods()
    {
        return InnerNetObjectTypes.Select(x => x.GetMethod("Awake", AccessTools.allDeclared)).Where(m => m != null)!;
    }

    public static void Postfix(MonoBehaviour __instance)
    {
        var manager = Main._Service.Get<InstanceManager>();
        foreach (var (key, value) in manager.Instances)
        {
            value?.RemoveAll(n => n == null);
        }
        manager.RegisterInstance(__instance);
    }
}