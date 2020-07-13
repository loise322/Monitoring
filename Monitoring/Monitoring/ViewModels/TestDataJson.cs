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
    public class TestDataJson
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
        /// Определение метрики.
        /// </summary>
        public string Kind { get; set; }
        /// <summary>
        /// Числовое значение метрики
        /// </summary>
        public int Value { get; set; }
    }
}
