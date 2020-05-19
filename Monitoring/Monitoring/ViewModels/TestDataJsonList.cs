using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoring.ViewModels
{
    /// <summary>
    /// Используется для передачи данных в представление.
    /// </summary>
    public class TestDataJsonList
    {
        /// <summary>
        /// Список метрик.
        /// </summary>
        public List<TestDataJson> Metrics { get; set; }
    }
}
