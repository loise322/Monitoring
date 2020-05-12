using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Monitoring.Models;
using Monitoring.ViewModels;
using Newtonsoft.Json;
using NLog;

namespace Monitoring.Classes
{
    /// <summary>
    /// Используется для сериализации и десереализации данных.
    /// </summary>
    public class JsonConverters
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Десериализация принятых данных.
        /// </summary>
        /// <param name="Data">Принятые JSON данные из представления.</param>
        public TestDataJson DeserializeTestData(JsonElement Data)
        {
            try
            {
                TestDataJson DeserealizedData = JsonConvert.DeserializeObject<TestDataJson>(Data.ToString());
                return DeserealizedData;
            }
            catch (Exception ex)
            {
                logger.Error($"При сериализации данных {Data.ToString()} произошла ошибка: " + ex);
                return null;
            };
        }

        /// <summary>
        /// Десериализация данных для редактирования и добавления метрик в базу данных.
        /// </summary>
        /// <param name="Data">Принятый JSON данные из представления.</param>
        public MetricItem DeserializeMetric(JsonElement Data)
        {
            try
            {
                MetricItem DeserealizedData = JsonConvert.DeserializeObject<MetricItem>(Data.ToString());
                return DeserealizedData;
            }
            catch (Exception ex)
            {
                logger.Error($"При сериализации данных {Data.ToString()} произошла ошибка: " + ex);
                return null;
            };
        }

        /// <summary>
        /// Сериализация ошибок валидатора для передечи в представление.
        /// </summary>
        /// <param name="errors">Список ошибок валидатора.</param>
        /// <returns>Возвращает ошибки валидатор в виде JSON</returns>
        public string SerializeErrors(List<string> errors)
        {
            try
            {
                string SerializedErrors = JsonConvert.SerializeObject(errors);
                return SerializedErrors;
            }
            catch (Exception ex)
            {
                logger.Error($"При сериализации данных {errors} произошла ошибка: " + ex);
                return null;
            };
        }
    }
}
