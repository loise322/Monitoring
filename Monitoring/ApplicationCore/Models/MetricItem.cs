using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationCore.Models
{
    /// <summary>
    /// Модель представляет те объекты, которые будут храниться в базе данных.
    /// </summary>
    public class MetricItem
    {
        /// <summary>
        /// Идентификатор метрики.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование метрики.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Является ли метрика логической.
        /// </summary>
        public bool IsBoolean { get; set; }
        /// <summary>
        /// Порог предупреждения.
        /// </summary>
        public int WarningThreshold { get; set; }
        /// <summary>
        /// Порог тревоги.
        /// </summary>
        public int AlarmThreshold { get; set; }
        /// <summary>
        /// Приоритет метрики для отображения.
        /// </summary>
        public PriorityKind Priority { get; set; }
        /// <summary>
        /// Идентификатор отслеживаемой переменной.
        /// </summary>
        public string Kind { get; set; }
    }
}
