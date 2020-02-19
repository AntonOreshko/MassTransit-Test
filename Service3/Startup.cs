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
using Service3.Consumers;

namespace Service3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<S3Event1Consumer>();
            services.AddScoped<S2Event2Consumer>();
            services.AddScoped<S3Event3Consumer>();

            var azureServiceBus = Bus.Factory.CreateUsingAzureServiceBus(configurator =>
            {
                configurator.SubscriptionEndpoint<Event1>(PathConstants.EVENT1, x => { x.Consumer<S3Event1Consumer>(); });
                configurator.SubscriptionEndpoint<Event2>(PathConstants.EVENT2, x => { x.Consumer<S2Event2Consumer>(); });
                configurator.SubscriptionEndpoint<Event3>(PathConstants.EVENT3, x => { x.Consumer<S3Event3Consumer>(); });

                configurator.Host(PathConstants.CONNECTION_STRING);
            });
            
            azureServiceBus.Start();
            
            ProbeResult result = azureServiceBus.GetProbeResult();

            Console.WriteLine(JsonConvert.SerializeObject(result));

            services.AddMassTransit(config =>
            {
                config.AddConsumer<S3Event1Consumer>();
                config.AddConsumer<S2Event2Consumer>();
                config.AddConsumer<S3Event3Consumer>();
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