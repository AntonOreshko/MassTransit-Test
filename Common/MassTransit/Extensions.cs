using System.IO;
using Common.Messages;
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
                configurator.Message<Event1>(configTopology => { configTopology.SetEntityName(TypeName<Event1>()); });
                configurator.Message<Event2>(configTopology => { configTopology.SetEntityName(TypeName<Event2>()); });
                configurator.Message<Event3>(configTopology => { configTopology.SetEntityName(TypeName<Event3>()); });
                
                configurator.Message<Command1>(configTopology => { configTopology.SetEntityName(TypeName<Command1>()); });
                configurator.Message<Command2>(configTopology => { configTopology.SetEntityName(TypeName<Command2>()); });
                configurator.Message<Command3>(configTopology => { configTopology.SetEntityName(TypeName<Command3>()); });

                configurator.Publish<Event1>(configTopology => { });
                configurator.Publish<Event2>(configTopology => { });
                configurator.Publish<Event2>(configTopology => { });
                
                configurator.Send<Command1>(configTopology => { });
                configurator.Send<Command2>(configTopology => { });
                configurator.Send<Command3>(configTopology => { });
                
                configurator.SubscriptionEndpoint<Event1>(TypeName<Event1>(), endpointConfigurator => {});
                configurator.SubscriptionEndpoint<Event2>(TypeName<Event2>(), endpointConfigurator => {});
                configurator.SubscriptionEndpoint<Event3>(TypeName<Event3>(), endpointConfigurator => {});
                
                configurator.ReceiveEndpoint(TypeName<Command1>(), endpointConfigurator => { });
                configurator.ReceiveEndpoint(TypeName<Command2>(), endpointConfigurator => { });
                configurator.ReceiveEndpoint(TypeName<Command3>(), endpointConfigurator => { });
                
                var host = configurator.Host(connectionString);
            }));
            
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
        }
        
        public static string TypeName<T>()
        {
            return typeof(T).FullName;
        }
    }
}