using System;
using System.Linq;
using Monitoring.Models;

namespace Monitoring
{
    public static class DebugTablesDbInit
    {
        public static void Initialize(TableContext context)
        {
            if (!context.Metrics.Any())
            {
                context.Metrics.AddRange(
                    new MetricItem { Name = "Name1", IsBoolean = false, WarningThreshold = 5, AlarmThreshold = 12, Priority = PriorityClass.Low, Kind = "Kind1"  },   
                    new MetricItem { Name = "Name2", IsBoolean = false, WarningThreshold = 60, AlarmThreshold = 90, Priority = PriorityClass.High, Kind = "Kind2" },
                    new MetricItem { Name = "Name3", IsBoolean = false, WarningThreshold = 30, AlarmThreshold = 120, Priority = PriorityClass.Medium, Kind = "Kind3" }
                );
                context.SaveChanges();
            }
            if (!context.Logs.Any())
            {
                context.Logs.AddRange(
                    new LogObject { Value = 0, Date = DateTime.Now, MetricId = 1 },
                    new LogObject { Value = 0, Date = DateTime.Now, MetricId = 2 },
                    new LogObject { Value = 0, Date = DateTime.Now, MetricId = 3 }
                );
                context.SaveChanges();
            }
        }
    }
}
