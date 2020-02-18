using System;
using Common.MassTransit;
using Common.Messages;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service1.Consumers;

namespace Service1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<Event1Consumer>();
            services.AddScoped<Event2Consumer>();
            services.AddScoped<Event3Consumer>();

            IBusControl CreateBus(IServiceProvider serviceProvider)
            {
                var bus = Bus.Factory.CreateUsingAzureServiceBus(configurator =>
                {
                    var connectionString =
                        "Endpoint=sb://sleipnir-dev-servicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=boXjqvXICJJdk5BvT93orbotaTssYmiLZY6PLBtdq+I=";

                    configurator.Message<Event1>(configTopology => { configTopology.SetEntityName(TypeName<Event1>()); });
                    configurator.Message<Event2>(configTopology => { configTopology.SetEntityName(TypeName<Event2>()); });
                    configurator.Message<Event3>(configTopology => { configTopology.SetEntityName(TypeName<Event3>()); });

                    //configurator.Message<Command1>(configTopology => { configTopology.SetEntityName(TypeName<Command1>()); });
                    // configurator.Message<Command2>(configTopology => { configTopology.SetEntityName(TypeName<Command2>()); });
                    // configurator.Message<Command3>(configTopology => { configTopology.SetEntityName(TypeName<Command3>()); });

                    configurator.Publish<Event1>(configTopology => { });
                    configurator.Publish<Event2>(configTopology => { });
                    configurator.Publish<Event2>(configTopology => { });
                    //
                    // configurator.Send<Command1>(configTopology => { });
                    // configurator.Send<Command2>(configTopology => { });
                    // configurator.Send<Command3>(configTopology => { });

                    
                    configurator.SubscriptionEndpoint<Event1>(TypeName<Event1>(), x =>
                    {
                        x.Consumer<Event1Consumer>();
                        //x.ConfigureConsumer<Event1Consumer>(serviceProvider);
                    });
                    
                    configurator.SubscriptionEndpoint<Event2>(TypeName<Event2>(), x =>
                    {
                        x.Consumer<Event2Consumer>();
                        x.ConfigureConsumer<Event2Consumer>(serviceProvider);
                    });
                    
                    configurator.SubscriptionEndpoint<Event3>(TypeName<Event3>(), x =>
                    {
                        x.Consumer<Event3Consumer>();
                        x.ConfigureConsumer<Event3Consumer>(serviceProvider);
                    });

                    configurator.Host(connectionString);
                });

                bus.Start();
                
                return bus;
            }
            
            void ConfigureMassTransit(IServiceCollectionConfigurator configurator)
            {
                configurator.AddConsumer<Event1Consumer>();
                configurator.AddConsumer<Event2Consumer>();
                configurator.AddConsumer<Event3Consumer>();
            }

            services.AddSingleton(CreateBus);
            
            services.AddMassTransit(ConfigureMassTransit);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var connectionString =
                "Endpoint=sb://sleipnir-dev-servicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=boXjqvXICJJdk5BvT93orbotaTssYmiLZY6PLBtdq+I=";

            var azureServiceBus = Bus.Factory.CreateUsingAzureServiceBus(configurator =>
            {
                // configurator.Message<Event1>(configTopology => { configTopology.SetEntityName(TypeName<Event1>()); });
                // configurator.Message<Event2>(configTopology => { configTopology.SetEntityName(TypeName<Event2>()); });
                // configurator.Message<Event3>(configTopology => { configTopology.SetEntityName(TypeName<Event3>()); });

                //configurator.Message<Command1>(configTopology => { configTopology.SetEntityName(TypeName<Command1>()); });
                // configurator.Message<Command2>(configTopology => { configTopology.SetEntityName(TypeName<Command2>()); });
                // configurator.Message<Command3>(configTopology => { configTopology.SetEntityName(TypeName<Command3>()); });

                // configurator.Publish<Event1>(configTopology => { });
                // configurator.Publish<Event2>(configTopology => { });
                // configurator.Publish<Event2>(configTopology => { });
                //
                // configurator.Send<Command1>(configTopology => { });
                // configurator.Send<Command2>(configTopology => { });
                // configurator.Send<Command3>(configTopology => { });

                configurator.SubscriptionEndpoint<Event1>(TypeName<Event1>(), x => { x.Consumer<Event1Consumer>(); });
                configurator.SubscriptionEndpoint<Event2>(TypeName<Event2>(), x => { x.Consumer<Event2Consumer>(); });
                configurator.SubscriptionEndpoint<Event3>(TypeName<Event3>(), x => { x.Consumer<Event3Consumer>(); });

                configurator.ReceiveEndpoint(TypeName<Command1>(), endpointConfigurator => { endpointConfigurator.Consumer<Command1Consumer>(); });
                configurator.ReceiveEndpoint(TypeName<Command2>(), endpointConfigurator => { endpointConfigurator.Consumer<Command2Consumer>(); });
                configurator.ReceiveEndpoint(TypeName<Command3>(), endpointConfigurator => { endpointConfigurator.Consumer<Command3Consumer>(); });

                var host = configurator.Host(connectionString);
            });

            services.AddSingleton<IPublishEndpoint>(azureServiceBus);
            services.AddSingleton<ISendEndpointProvider>(azureServiceBus);
            services.AddSingleton<IBus>(azureServiceBus);

            services.AddMassTransit(config => { config.AddBus(provider => azureServiceBus); });
        }

        private static string TypeName<T>()
        {
            return typeof(T).FullName;
        }
    }
}