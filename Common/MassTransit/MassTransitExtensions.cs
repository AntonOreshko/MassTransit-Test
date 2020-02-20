using System;
using System.Linq;
using Common.MassTransit.Services;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Common.MassTransit
{
    public static class MassTransitExtensions
    {
        public static void ConfigureMassTransit(this IServiceCollection services, Action<IServiceBusBusFactoryConfigurator>[] configurators, IConfiguration configuration)
        {
            var settings = new MassTransitSettings();
            var section = configuration.GetSection("MassTransitSettings");
            section.Bind(settings);
            services.Configure<MassTransitSettings>(section);
            
            var azureServiceBus = Bus.Factory.CreateUsingAzureServiceBus(internalConfigurator =>
            {
                configurators.ToList().ForEach(configurator =>
                {
                    configurator(internalConfigurator);
                });
                
                internalConfigurator.Host(settings.AzureServiceBusConnectionString);
            });
            
            azureServiceBus.Start();
            
            services.AddSingleton(azureServiceBus);

            services.AddScoped<IMassTransitService, MassTransitService>();
        }

        public static Action<IServiceBusBusFactoryConfigurator> DeclareCommand<TMessage>(this object obj, string queueName = null)
            where TMessage : class
        {
            return configurator =>
            {
                if (string.IsNullOrEmpty(queueName)) queueName = TypeName<TMessage>();
                configurator.Message<TMessage>(x => x.SetEntityName(queueName));
            };
        }

        public static Action<IServiceBusBusFactoryConfigurator> AddEventSubscription<TConsumer, TMessage>(this object obj, string subscriptionName, string topicName = null)
            where TConsumer: class, IConsumer<TMessage>, new()
            where TMessage : class
        {
            return configurator =>
            {
                if (string.IsNullOrEmpty(topicName)) topicName = TypeName<TMessage>();
                configurator.Message<TMessage>(x => x.SetEntityName(topicName));
                configurator.SubscriptionEndpoint<TMessage>(subscriptionName, x => x.Consumer<TConsumer>());
            };
        }

        public static Action<IServiceBusBusFactoryConfigurator> AddCommandSubscription<TConsumer, TMessage>(this object obj, string queueName = null)
            where TConsumer: class, IConsumer<TMessage>, new()
            where TMessage : class
        {
            return configurator =>
            {
                if (string.IsNullOrEmpty(queueName)) queueName = TypeName<TMessage>();
                configurator.ReceiveEndpoint(queueName, x => x.Consumer<TConsumer>());
            };
        }

        public static void AddCommandRequestClient<TRequest, TResponse>(this IServiceCollection services, IConfiguration configuration)
            where TRequest : class 
            where TResponse : class
        {
            var settings = new MassTransitSettings();
            var section = configuration.GetSection("MassTransitSettings");
            section.Bind(settings);

            services.AddScoped<IRequestClient<TRequest, TResponse>>
            (
                x => new MessageRequestClient<TRequest, TResponse>
                (
                    x.GetRequiredService<IBusControl>(), 
                    new Uri(settings.AzureServiceBusUrl + TypeName<TRequest>()), 
                    TimeSpan.FromSeconds(settings.RequestTimeout), 
                    TimeSpan.FromSeconds(settings.RequestTimeToLive)
                )
            );
        }
        
        public static string TypeName<T>(this object obj)
        {
            return typeof(T).FullName;
        }
        
        private static string TypeName<T>()
        {
            return typeof(T).FullName;
        }
    }
}