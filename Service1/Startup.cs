using Common.MassTransit;
using Common.Messages;
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
            
            services.ConfigureMassTransit( new[]
            {
                this.AddEventSubscription<S1Event1Consumer, Event1>("service-1-event-1"),
                this.AddEventSubscription<S1Event2Consumer, Event2>("service-1-event-2"),
                this.AddEventSubscription<S1Event3Consumer, Event3>("service-1-event-3"),
                this.DeclareCommand<Command1>(),
                this.DeclareCommand<Request1>(),
                this.DeclareCommand<Request2>()
            }, Configuration);

            services.AddCommandRequestClient<Request1, Response1>(Configuration);
            services.AddCommandRequestClient<Request2, Response2>(Configuration);
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