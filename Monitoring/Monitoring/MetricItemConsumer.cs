using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using MassTransit;

namespace Monitoring
{
    public class MetricItemConsumer : IConsumer<MetricItem>
    {
        public async Task Consume(ConsumeContext<MetricItem> context)
        {
            var data = context.Message;
            // message received.
        }
    }
}
