using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monitoring.Models;

namespace Monitoring.ViewModels
{
    /// <summary>
    /// Используется для передачи данных в представление.
    /// </summary>
    public class GraphicModel
    {
        /// <summary>
        /// Идентификатор метрики.
        /// </summary>
        public int MetricId { get; set; }
        public IEnumerable<int> Values { get; set; }
        public IEnumerable<int> Labels { get; set; }
    }
}
