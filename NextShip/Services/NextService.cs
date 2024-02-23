using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NextShip.Api.Interfaces;

namespace NextShip.Services;

public class NextService : INextService
{
    private static readonly HashSet<NextService> Services = [];

    public int id;

    public NextService()
    {
        Services.Add(this);
        id = Services.Count == 0 ? 0 : Services.Last().id++;
    }

    public NextService(IServiceProvider provider) : this()
    {
        _Provider = provider;
        BuildCompleted = true;
    }

    public IServiceProvider _Provider { get; private set; }

    private bool BuildCompleted { get; set; }

    public void Build()
    {
        if (BuildCompleted) return;

        var collection = new ServiceCollection();

        _Provider = collection.BuildServiceProvider();
    }


    public void Rebuild()
    {
        Reset();
        Build();
    }

    public void Reset()
    {
        _Provider = null;
        BuildCompleted = false;
    }

    public static NextService Build(IServiceBuilder builder)
    {
        var service = builder.Build();
        service.Build();

        return (NextService)service;
    }

    public static NextService Get(int id) => Services.FirstOrDefault(n => n.id == id);

    public T Get<T>()
    {
        return _Provider!.GetService<T>();
    }

    public object Get(Type type)
    {
        return _Provider!.GetService(type);
    }

    public static NextService Build(ServiceCollection collection)
    {
        return (NextService)new ServiceBuilder().Set(collection).Build();
    }
}

internal static class ServiceExt
{
    internal static T ServiceGet<T>() => Main._Service.Get<T>();

    internal static object ServiceGet(this Type type) => Main._Service.Get(type);
    
    internal static IEnumerable<T> ServiceGets<T>() => Main._Service._Provider.GetServices<T>();
    
    internal static IEnumerable<object> ServiceGets(this Type type) => Main._Service._Provider.GetServices(type);
}