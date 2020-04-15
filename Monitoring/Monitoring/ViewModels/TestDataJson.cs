using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monitoring.Models;

namespace Monitoring.ViewModels
{
    public class TestDataJson
    {
        public string Name { get; set; }
        public bool IsBoolean { get; set; }
        public int WarningThreshold { get; set; }
        public int AlarmThreshold { get; set; }
        public PriorityClass Priority { get; set; }
        public string Kind { get; set; }
        public int Value { get; set; }
    }
}
