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
        /// <summary>
        /// Название метрики
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Значения метрики для графика
        /// </summary>
        public IEnumerable<int> Values { get; set; }
        /// <summary>
        /// Ярлыки метрики для графика
        /// </summary>
        public IEnumerable<int> Labels { get; set; }
    }
}
