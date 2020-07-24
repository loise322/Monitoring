using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using MassTransit;
using Monitoring.ViewModels;
using NLog;

namespace Monitoring
{
    public class MetricItemConsumer : IConsumer<TestDataJson[]>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public async Task Consume(ConsumeContext<TestDataJson[]> context)
        {
            var data = context.Message;
            logger.Info($"Пришли данные с RabbitMq: {context.Message}");
            await Task.CompletedTask;
        }
    }
}
