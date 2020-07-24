using System;
using System.Threading;
using System.Threading.Tasks;
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

                var endpoint = await busControl.GetSendEndpoint(new Uri("exchange:account-service"));

                await endpoint.Send<TestDataJson>(new
                {
                   Id = 0,
                   Name = "TestRabbit",
                   IsBoolean = false,
                   WarningThreshold = 20,
                   AlarmThreshold = 30,
                   Priority = 1,
                   Kind = "testRabbit",
                   Value = 25
                });
            }
            finally
            {
                await busControl.StopAsync(CancellationToken.None);
            }
        }
    }
}
