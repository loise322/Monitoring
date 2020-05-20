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
    public class DataConverter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Десериализация принятых данных.
        /// </summary>
        /// <param name="data">Принятые JSON данные из представления.</param>
        public TestDataJson DeserializeTestData(JsonElement data)
        {
            try
            {
                TestDataJson deserealizedData = JsonConvert.DeserializeObject<TestDataJson>(data.ToString());
                return deserealizedData;
            }
            catch (Exception ex)
            {
                logger.Error($"При сериализации данных {data.ToString()} произошла ошибка: " + ex);
                return null;
            };
        }

        /// <summary>
        /// Десериализация данных для редактирования и добавления метрик в базу данных.
        /// </summary>
        /// <param name="data">Принятый JSON данные из представления.</param>
        public MetricItem DeserializeMetric(JsonElement data)
        {
            try
            {
                MetricItem deserealizedData = JsonConvert.DeserializeObject<MetricItem>(data.ToString());
                return deserealizedData;
            }
            catch (Exception ex)
            {
                logger.Error($"При сериализации данных {data.ToString()} произошла ошибка: " + ex);
                return null;
            };
        }

        /// <summary>
        /// Сериализация строк для передечи в представление.
        /// </summary>
        /// <param name="strings">Список строк.</param>
        /// <returns>Возвращает список строк в виде JSON строки.</returns>
        public string SerializeStrings(List<string> strings)
        {
            try
            {
                string serializedStrings = JsonConvert.SerializeObject(strings);
                return serializedStrings;
            }
            catch (Exception ex)
            {
                logger.Error($"При сериализации данных {strings} произошла ошибка: " + ex);
                return null;
            };
        }
    }
}
