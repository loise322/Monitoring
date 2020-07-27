using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Infrastucture.RabbitMQService;
using MassTransit;


namespace TestRabbitMq
{
    public class MetricsRequiredConsumer : IConsumer<MetricsRequiredNotification>
    {
        public async Task Consume(ConsumeContext<MetricsRequiredNotification> context)
        {
            var data = context.Message;
            Random rnd = new Random();
            var metrics = new List<PreparedMetrics>();
            foreach (var item in data.SpecificMetricKinds)
            {
                metrics.Add(new PreparedMetrics 
                { 
                    Kind = item,
                    Value = rnd.Next(0,20)
                });
            };
            var metricsNotification = new MetricsNotification
            {
                CallingApplication = context.Message.CallingApplication,
                SourceApplication = "OutsideService",
                TimestampUtc = DateTime.Now,
                RequestTimestampUtc = context.Message.TimestampUtc,
                Metrics = metrics.ToArray()
            };
            var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/test-data-prepared"));
            await endpoint.Send(metricsNotification);
        }
    }
}
