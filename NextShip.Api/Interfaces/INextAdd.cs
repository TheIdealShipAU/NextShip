using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using NextShip.Api.Services;

namespace NextShip.Api.Interfaces;

public interface INextAdd
{
    public void ServiceAdd(IServiceBuilder serviceBuilder)
    {
    }

    public void DependentAdd(DependentService dependentService)
    {
    }

    public void ConfigBind(ConfigFile config)
    {
    }

    public static List<INextAdd> GetAdds(Assembly assembly)
    {
        var list = new List<INextAdd>();

        assembly.GetTypes().Where(n => n.IsDefined(typeof(INextAdd)))
            .Select(AccessTools.CreateInstance)
            .Select(n => (INextAdd)n)
            .Do(list.Add);

        return list;
    }
}