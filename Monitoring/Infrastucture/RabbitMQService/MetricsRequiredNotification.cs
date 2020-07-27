using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastucture.RabbitMQService
{
    public class MetricsRequiredNotification
    {
        public string CallingApplication { get; set; }

        public string SourceApplication { get; set; }

        public DateTime TimestampUtc { get; set; }

        public string[] SpecificMetricKinds { get; set; }
    }
}
