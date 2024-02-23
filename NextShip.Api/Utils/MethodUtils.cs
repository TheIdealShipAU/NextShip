#nullable enable
using System.Reflection;

namespace NextShip.Api.Utils;

public static class MethodUtils
{
    /// <summary>
    ///     获取运行方法命名空间
    /// </summary>
    public static string GetNamespace()
    {
        return MethodBase.GetCurrentMethod()?.DeclaringType?.Namespace ?? string.Empty;
    }

    /// <summary>
    ///     获取运行方法
    /// </summary>
    public static string? GetVoidName()
    {
        return MethodBase.GetCurrentMethod()?.Name;
    }

    /// <summary>
    ///     获取运行方法类
    /// </summary>
    /// <returns></returns>
    public static string? GetClassName()
    {
        return MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
    }

    public static bool Is<T>(this MemberInfo info) where T : Attribute
    {
        return info.GetCustomAttribute<T>() != null;
    }

    public static bool IsD(this MemberInfo info, Type type)
    {
        return info.IsDefined(type);
    }

    public static bool IsD<T>(this MemberInfo info)
    {
        return info.IsD(typeof(T));
    }

    public static IEnumerable<T> GetMembers<T>(this Type type, Func<T, bool>? Is)
    {
        var memberInfos = type.GetMembers();

        return Is == null ? memberInfos.OfType<T>() : memberInfos.OfType<T>().Where(Is);
    }

    public static List<FieldInfo> GetFieldInfos(this Assembly assembly)
    {
        var types = assembly.GetTypes();

        return types.SelectMany(varType => varType.GetFields()).ToList();
    }
    
    public static List<MethodInfo> GetMethodInfos(this Assembly assembly)
    {
        var types = assembly.GetTypes();

        return types.SelectMany(varType => varType.GetMethods()).ToList();
    }
    
    public static List<ConstructorInfo> GetConstructorInfos(this Assembly assembly)
    {
        var types = assembly.GetTypes();

        return types.SelectMany(varType => varType.GetConstructors()).ToList();
    }
    
    public static List<MemberInfo> GetMemberInfos(this Assembly assembly)
    {
        var types = assembly.GetTypes();

        return types.SelectMany(varType => varType.GetMembers()).ToList();
    }
}