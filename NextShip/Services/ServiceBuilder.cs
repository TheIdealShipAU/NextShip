using System;
using Microsoft.Extensions.DependencyInjection;
using NextShip.Api.Interfaces;

namespace NextShip.Services;

public class ServiceBuilder : IServiceBuilder
{
    public ServiceCollection _collection { get; set; }

    public IServiceBuilder CreateService()
    {
        _collection = [];
        return this;
    }

    public IServiceBuilder Add<T>() where T : class
    {
        _collection.AddSingleton<T>();
        return this;
    }

    public IServiceBuilder Add(Type type)
    {
        _collection.AddSingleton(type);
        return this;
    }

    public IServiceBuilder Add<TService>(Func<IServiceProvider, TService> func) where TService : class
    {
        _collection.AddSingleton(func);
        return this;
    }

    public INextService Build()
    {
        return new NextService(_collection.BuildServiceProvider());
    }

    public IServiceBuilder Set(ServiceCollection collection)
    {
        _collection = collection;
        return this;
    }

    public IServiceBuilder AddLogging()
    {
        _collection.AddLogging();
        return this;
    }

    public IServiceBuilder AddScoped<T>() where T : class
    {
        _collection.AddScoped<T>();
        return this;
    }

    public IServiceBuilder AddScoped(Type type)
    {
        _collection.AddScoped(type);
        return this;
    }

    public IServiceBuilder AddTransient<T>() where T : class
    {
        _collection.AddTransient<T>();
        return this;
    }

    public IServiceBuilder AddTransient(Type type)
    {
        _collection.AddTransient(type);
        return this;
    }
}