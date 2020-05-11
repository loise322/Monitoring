using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monitoring.Models
{
    /// <summary>
    /// Модель представляет те объекты, которые будут храниться в базе данных.
    /// </summary>
    public class LogObject
    {
        /// <summary>
        /// Идентификатор лога.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Численное значение лога.
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Время добавления лога
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Идентификатор метрики, к которой относится лог.
        /// </summary>
        public int MetricId { get; set; }
    }
}
