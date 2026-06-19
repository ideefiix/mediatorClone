using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatorClone.Internal;

namespace MediatorClone.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IPublisher, Mediator>();

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract))
            {
                foreach (var @interface in type.GetInterfaces().Where(i => i.IsGenericType))
                {
                    if (@interface.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                        services.AddScoped(@interface, type);
                }
            }
        }
        return services;
    }
}
