using System;
using Common.Constants;
using Common.MassTransit;
using Common.Messages;
using GreenPipes;
using GreenPipes.Introspection;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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

            services.AddScoped<S1Event1Consumer>();
            services.AddScoped<S1Event2Consumer>();
            services.AddScoped<S1Event3Consumer>();

            var azureServiceBus = Bus.Factory.CreateUsingAzureServiceBus(configurator =>
            {
                configurator.Message<Event1>(x => x.SetEntityName(PathConstants.EVENT1));
                configurator.Publish<Event1>(x => {});

                configurator.SubscriptionEndpoint<Event1>(PathConstants.EVENT1, x =>
                {
                    x.Consumer<S1Event1Consumer>();
                    x.Consumer<S1Event2Consumer>();
                    x.Consumer<S1Event3Consumer>();
                });
                configurator.SubscriptionEndpoint<Event2>(PathConstants.EVENT2, x =>
                {
                    x.Consumer<S1Event1Consumer>();
                    x.Consumer<S1Event2Consumer>();
                    x.Consumer<S1Event3Consumer>();
                });
                configurator.SubscriptionEndpoint<Event3>(PathConstants.EVENT3, x =>
                {
                    x.Consumer<S1Event1Consumer>();
                    x.Consumer<S1Event2Consumer>();
                    x.Consumer<S1Event3Consumer>();
                });
                configurator.ReceiveEndpoint(PathConstants.COMMAND1, x =>
                {
                    x.Consumer<Command1Consumer>();
                });
                configurator.ReceiveEndpoint(PathConstants.COMMAND2, x =>
                {
                    x.Consumer<Command2Consumer>();
                });
                configurator.ReceiveEndpoint(PathConstants.COMMAND3, x =>
                {
                    x.Consumer<Command3Consumer>();
                });

                
                configurator.Host(PathConstants.CONNECTION_STRING);
            });
            
            azureServiceBus.Start();
            
            ProbeResult result = azureServiceBus.GetProbeResult();

            Console.WriteLine(JsonConvert.SerializeObject(result));
            
            services.AddMassTransit(config =>
            {
                config.AddConsumer<S1Event1Consumer>();
                config.AddConsumer<S1Event2Consumer>();
                config.AddConsumer<S1Event3Consumer>();
            });

            services.AddSingleton(azureServiceBus);
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
    }
}