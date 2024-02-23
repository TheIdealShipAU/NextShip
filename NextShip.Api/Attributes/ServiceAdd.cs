using System.Reflection;
using HarmonyLib;

namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ServiceAddAttribute : Attribute
{
    private readonly object _instance;
    
    public ServiceAddAttribute()
    {
    }

    public ServiceAddAttribute(object instance)
    {
        _instance = instance;
    }
    
    
    public static void Registration(IServiceProvider provider, Assembly assembly)
    {
        var Fields = assembly.GetFieldInfos();
        var methods = assembly.GetMethodInfos();
        var constructors = assembly.GetConstructorInfos();
        
        foreach (var Var in Fields.Where(n => n.Is<ServiceAddAttribute>()))
        {
            var instance = Var.GetCustomAttribute<ServiceAddAttribute>()?._instance;
            if (!Var.IsStatic && instance == null) continue;

            var type = Var.FieldType;
            var value = provider.GetService(type);
            Var.SetValue(null, value);
        }
        
        foreach (var Var in methods.Where(n => n.Is<ServiceAddAttribute>()))
        {
            var instance = Var.GetCustomAttribute<ServiceAddAttribute>()?._instance;
            if (!Var.IsStatic && instance == null) continue;

            var arguments = Var.GetGenericArguments().Select(provider.GetService).ToArray();
            _ = Var.Invoke(instance, arguments);
        }
        
        foreach (var Var in constructors.Where(n => n.Is<ServiceAddAttribute>()))
        {
            var arguments = Var.GetGenericArguments().Select(provider.GetService).ToArray();
            _ = Var.Invoke(null, arguments);
        }
    }
}