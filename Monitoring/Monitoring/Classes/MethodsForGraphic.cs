using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoring.Classes
{
    /// <summary>
    /// Класс методов для заполнения данных для график.
    /// </summary>
    public class MethodsForGraphic
    {
        /// <summary>
        /// Метод FillLabels() используется для заполнения ярлыков для графика.
        /// </summary>
        /// <param name="ConditionValue">Аргумент метода FillLabels(). Устанавливает кол-во ярлыков.</param>
        /// <returns></returns>
        public List<int> FillLabels(int ConditionValue)
        {
            List<int> labels = new List<int>();
            for (int i = 0; i < ConditionValue; i++)
            {
                labels.Add(i + 1);
            };
            return labels;
        }
    }
}
