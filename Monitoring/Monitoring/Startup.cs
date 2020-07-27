using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Monitoring.Services;
using ApplicationCore.Validators;
using MassTransit;
using GreenPipes;
using System.Threading;
using System;
using Infrastucture.RabbitMQService;

namespace Monitoring
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public  void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TableContext>(options => options.UseSqlServer(connection));
            services.AddScoped<IMetricService, MetricService>();
            services.AddScoped<IProcessingData, ProcessingData>();
            services.AddScoped<IGraphicService, GraphicService>();
            services.AddScoped<IProcessingViewModels, ProcessingViewModels>();
            services.AddScoped<IDataConverter, DataConverter>();
            services.AddScoped<ITestingApp, TestingApp>();
            services.AddControllersWithViews();

            services.AddScoped<MetricItemConsumer>();
            services.AddMassTransit(x =>
            {
                // add the consumer to the container
                x.AddConsumer<MetricItemConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("test-data-prepared", e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));

                    e.Consumer<MetricItemConsumer>(provider);

                    EndpointConvention.Map<MetricsNotification>(e.InputAddress);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<MetricsNotification>());

            services.AddSingleton<IHostedService, BusService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=View}/{action=Metrics}/{id?}");
            });
        }
    }
}
