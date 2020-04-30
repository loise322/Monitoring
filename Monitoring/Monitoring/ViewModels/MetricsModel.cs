using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monitoring.Models;

namespace Monitoring.ViewModels
{
    public class MetricsModel
    {
        public IEnumerable<MetricItem> Metrics { get; set; }
    }
}
