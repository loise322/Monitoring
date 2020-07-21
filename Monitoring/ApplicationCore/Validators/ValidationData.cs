using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Validators
{
    /// <summary>
    /// Данные для валидации.
    /// </summary>
    public class ValidationData
    {
        /// <summary>
        /// Наименование валидируемой строки.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Значение строки.
        /// </summary>
        public string Value { get; set; }

        public int WarningThreshold { get; set; }

        public int AlarmThreshold { get; set; }
        /// <summary>
        /// Тип валидации, которую строка должна пройти.
        /// </summary>
        public ValidationKind Kind { get; set; }


    }
}
