using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace Monitoring.ViewModels
{
    /// <summary>
    /// Используется для передачи данных в представление.
    /// </summary>
    public class MetricsModel
    {
        /// <summary>
        /// Список метрик.
        /// </summary>
        public IEnumerable<MetricItem> Metrics { get; set; }
    }
}
