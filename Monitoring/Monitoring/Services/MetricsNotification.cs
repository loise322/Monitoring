using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public class MetricsNotification
    {
        public string CallingApplication { get; set; }

        public string SourceApplication { get; set; }

        public DateTime RequestTimestampUtc { get; set; }

        public DateTime TimestampUtc { get; set; }

        public TestDataJson[] Metrics { get; set; }
    }
}
