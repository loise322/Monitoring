using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monitoring.Models
{
    public class MetricItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsBoolean { get; set; }
        public int WarningThreshold { get; set; }
        public int AlarmThreshold { get; set; }
        public PriorityKind Priority { get; set; }
        public string Kind { get; set; }
    }
}
