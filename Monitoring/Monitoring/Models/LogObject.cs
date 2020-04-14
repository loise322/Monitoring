using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monitoring.Models
{
    public class LogObject
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public int MetricId { get; set; }
    }
}
