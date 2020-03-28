﻿using System;
using System.Linq;
using Monitoring.Models;

namespace Monitoring
{
    public static class DebugTablesDbInit
    {
        public static void Initialize(TableContext context)
        {
            if (!context.Vocs.Any())
            {
                context.Vocs.AddRange(
                    new VocOfVariable { Name = "Name", isBoolean = false, warningThreshold = 5, alarmThreshold = 12, Priority = 1 }
                    
                );
                context.SaveChanges();
            }
            if (!context.Logs.Any())
            {
                context.Logs.AddRange(
                    new LogObject { Value = 0, Date = DateTime.Now.ToShortDateString() }
                );
                context.SaveChanges();
            }
        }
    }
}
