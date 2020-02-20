using Common.MassTransit;
using Common.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.ConfigureMassTransit( new[]
            {
                services.SubscribeToTopic<S3Event1Consumer, Event1>("service-3-event-1"),
                services.SubscribeToTopic<S3Event2Consumer, Event2>("service-3-event-2"),
                services.SubscribeToTopic<S3Event3Consumer, Event3>("service-3-event-3"),
                services.SubscribeToQueue<Command1Consumer, Command1>()
            }, Configuration);
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