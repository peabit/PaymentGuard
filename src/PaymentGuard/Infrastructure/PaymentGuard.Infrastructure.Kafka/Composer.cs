using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PaymentGuard.Infrastructure.Kafka;

public static class Composer
{
    public static IServiceCollection AddKafka(this IServiceCollection services, Assembly assembly)
    {
        foreach (var (interfaceType, implementationType) in assembly.GetHandlerTypes())
        {
            services
                .AddSingleton(interfaceType, implementationType)
                .AddSingleton(typeof(IHostedService), interfaceType.GetHostedServiceType());
        }

        return services;
    }

    private static IEnumerable<(Type interfaceType, Type implementationType)> GetHandlerTypes(this Assembly assembly)
    {
        var types = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (var type in types)
        {
            var interfaceType = type
                .GetInterfaces()
                .SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageHandler<>));

            if (interfaceType is not null)
                yield return (interfaceType, implementationType: type);
        }
    }

    private static Type GetHostedServiceType(this Type type)
    {
        var messageType = type.GetGenericArguments()[0];

        return typeof(MessageHandlerHostedService<>).MakeGenericType(messageType);
    }
}