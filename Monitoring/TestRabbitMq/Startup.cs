using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using Infrastructure;
using Infrastucture.RabbitMQService;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace TestRabbitMq
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MassTransit to the service collection
            services.AddMassTransit();

            // Register and configure the bus
            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("test-data-required", e =>
                {
                    e.PrefetchCount = 16;

                    e.Consumer<MetricsRequiredConsumer>();

                    e.UseMessageRetry(x => x.Interval(2, 100));
                });
            }));

            // Register IBus so that controllers can specify the dependency in the constructor
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            // Register IPublishEndpoint
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());

            // Register hosted service using the interface type IHostedService to start/stop the bus with the application
            services.AddSingleton<IHostedService, BusService>();
            services.AddControllers();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
