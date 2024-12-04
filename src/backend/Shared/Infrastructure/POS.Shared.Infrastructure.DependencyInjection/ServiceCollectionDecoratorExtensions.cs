using Microsoft.Extensions.DependencyInjection;

namespace POS.Shared.Infrastructure.DependencyInjection;

/// <summary>
/// Extension methods for adding services with decorator.
/// </summary>
public static class ServiceCollectionDecoratorExtensions
{
    #region With one decorator.

    /// <summary>
    /// Registers a service with a transient lifetime, wrapping its implementation with a decorator.
    /// </summary>
    /// <typeparam name="TService">The service interface type.</typeparam>
    /// <typeparam name="TImpl">The implementation type of the service.</typeparam>
    /// <typeparam name="TDecorator">The decorator type for the service.</typeparam>
    /// <param name="services">The DI container to configure.</param>
    /// <returns>The updated DI container.</returns>
    public static IServiceCollection AddTransient<TService, TImpl, TDecorator>(this IServiceCollection services)
    where TService : class
    where TImpl : class, TService
    where TDecorator : TService
    {
        return services
            .AddTransient<TService, TImpl>()
            .Decorate<TService, TDecorator>();
    }

    /// <summary>
    /// Registers a service with a scoped lifetime, wrapping its implementation with a decorator.
    /// </summary>
    /// <typeparam name="TService">The service interface type.</typeparam>
    /// <typeparam name="TImpl">The implementation type of the service.</typeparam>
    /// <typeparam name="TDecorator">The decorator type for the service.</typeparam>
    /// <param name="services">The DI container to configure.</param>
    /// <returns>The updated DI container.</returns>
    public static IServiceCollection AddScoped<TService, TImpl, TDecorator>(this IServiceCollection services)
    where TService : class
    where TImpl : class, TService
    where TDecorator : TService
    {
        return services
            .AddScoped<TService, TImpl>()
            .Decorate<TService, TDecorator>();
    }

    /// <summary>
    /// Registers a service with a singleton lifetime, wrapping its implementation with a decorator.
    /// </summary>
    /// <typeparam name="TService">The service interface type.</typeparam>
    /// <typeparam name="TImpl">The implementation type of the service.</typeparam>
    /// <typeparam name="TDecorator">The decorator type for the service.</typeparam>
    /// <param name="services">The DI container to configure.</param>
    /// <returns>The updated DI container.</returns>
    public static IServiceCollection AddSingleton<TService, TImpl, TDecorator>(this IServiceCollection services)
    where TService : class
    where TImpl : class, TService
    where TDecorator : TService
    {
        return services
            .AddSingleton<TService, TImpl>()
            .Decorate<TService, TDecorator>();
    }

    #endregion
}
