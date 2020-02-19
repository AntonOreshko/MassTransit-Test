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
using Service2.Consumers;

namespace Service2
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

            services.ConfigureMassTransit( new[]
            {
                services.SubscribeToTopic<S2Event1Consumer, Event1>("service-2-event-1"),
                services.SubscribeToTopic<S2Event2Consumer, Event2>("service-2-event-2"),
                services.SubscribeToTopic<S2Event3Consumer, Event3>("service-2-event-3")
            });
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