using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monitoring.Models;

namespace Monitoring.ViewModels
{
    public class VariableAndLogsViewModel
    {
        public int LastValueOfLogs { get; set; }
        public IEnumerable<LogObject> Logs { get; set; }
        public IEnumerable<MetricItem> Metrics { get; set; }
    }
}
