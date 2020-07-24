using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Infrastructure;
using NLog;
using MassTransit;
using GreenPipes;
using Monitoring.Services;

namespace Monitoring
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (IServiceScope scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<TableContext>();
                    DebugTablesDbInit.Initialize(context);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Add MassTransit to the service collection and configure service collection
                    services.AddMassTransit(cfg =>
                    {
                        // Add bus to the collection
                        cfg.AddBus(ConfigureBus);
                        // Add consumer to the collection
                        cfg.AddConsumer<MetricItemConsumer>();
                    });

                    // Add IHostedService registration of type BusService
                    services.AddHostedService<BusService>();
                });

        private static IBusControl ConfigureBus(IServiceProvider provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("test-data-json", e =>
                {
                    e.PrefetchCount = 16;

                    e.UseMessageRetry(x => x.Interval(2, 100));

                    e.Consumer<MetricItemConsumer>(provider);
                });
            });
            
        }
    }
}
