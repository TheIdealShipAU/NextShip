using Microsoft.Extensions.DependencyInjection;

namespace NextShip.Api.Interfaces;

public interface IServiceBuilder
{
    public ServiceCollection _collection { get; set; }

    public IServiceBuilder CreateService();

    public IServiceBuilder Add<T>() where T : class;

    public IServiceBuilder Add<TService>(Func<IServiceProvider, TService> func) where TService : class;

    public IServiceBuilder Add(Type type);

    public INextService Build();

    public IServiceBuilder Set(ServiceCollection collection);

    public IServiceBuilder AddLogging();

    public IServiceBuilder AddScoped<T>() where T : class;

    public IServiceBuilder AddScoped(Type type);

    public IServiceBuilder AddTransient<T>() where T : class;

    public IServiceBuilder AddTransient(Type type);
}