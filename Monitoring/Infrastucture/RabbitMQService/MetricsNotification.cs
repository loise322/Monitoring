using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastucture.RabbitMQService
{
    public class MetricsNotification
    {
        public string CallingApplication { get; set; }

        public string SourceApplication { get; set; }

        public DateTime RequestTimestampUtc { get; set; }

        public DateTime TimestampUtc { get; set; }

        public PreparedMetrics[] Metrics { get; set; }
    }
}
