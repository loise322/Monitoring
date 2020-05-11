using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoring.Models
{
    /// <summary>
    /// Перечисление приоритетов. Используется для задания приоритета метрикам.
    /// </summary>
    public enum PriorityKind
    {
        High, Medium, Low
    }
}
