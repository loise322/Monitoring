using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastructure;
using Infrastucture.RabbitMQService;
using MassTransit;
using Monitoring.Services;
using Monitoring.ViewModels;
using NLog;

namespace Monitoring
{
    public class MetricItemConsumer : IConsumer<MetricsNotification>
    {
        private readonly IProcessingData _processingData;

        public MetricItemConsumer(IProcessingData processingData) 
        {
            _processingData = processingData;
        }

        public async Task Consume(ConsumeContext<MetricsNotification> context)
        {             
            var data = context.Message;
            _processingData.StartProcessingData(data.Metrics);
            await Task.CompletedTask;
        }
    }
}
