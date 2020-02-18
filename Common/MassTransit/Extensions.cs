using System.IO;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Common.MassTransit
{
    public static class Extensions
    {
        public static void RegisterMassTransit(this IServiceCollection services)
        {
            var connectionString = "Endpoint=sb://sleipnir-dev-servicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=boXjqvXICJJdk5BvT93orbotaTssYmiLZY6PLBtdq+I=";
            
            services.AddSingleton(serviceProvider => Bus.Factory.CreateUsingAzureServiceBus(configurator =>
            {
                var host = configurator.Host(connectionString);
            }));
            
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
        }
    }
}