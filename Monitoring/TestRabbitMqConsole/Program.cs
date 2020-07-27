using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models;
using MassTransit;

namespace TestRabbitMqConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("account-service", e =>
                {
                    e.Lazy = true;

                    e.PrefetchCount = 20;
                });
            });

            using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            await busControl.StartAsync(cancellation.Token);
            try
            {
                Console.WriteLine("Bus was started.");

              
            }
            finally
            {
                await busControl.StopAsync(CancellationToken.None);
            }
        }
    }
}
