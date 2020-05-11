using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Monitoring.Models;
using Monitoring.ViewModels;
using Newtonsoft.Json;
using NLog;

namespace Monitoring.ValidatorRepository
{
    /// <summary>
    /// Валидатор для сериализации, десереализации и проверки принятых данных на корректность
    /// Имеет методы DeserializationRequest(), DeserializationMetric(), StringsValidation(), StringValidator().
    /// </summary>
    public class Validator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Метод DeserializationRequest() используется для десериализации (а также её валидации) принятых данных, которые приходят в виде:
        /// {"name":string,"isBoolean":bool,"warningThreshold":int,"alarmThreshold":int,"priority":enum(0,2),"kind":string,"value":int}
        /// </summary>
        /// <param name="Data">Аргумент метода SerializationRequest()</param>
        public TestDataJson DeserializationRequest(JsonElement Data)
        {
            try
            {
                TestDataJson SerealizedData = JsonConvert.DeserializeObject<TestDataJson>(Data.ToString());
                return SerealizedData;
            }
            catch (Exception ex)
            {
                logger.Error($"При сериализации данных {Data.ToString()} произошла ошибка: " + ex);
                return null;
            };
        }
        /// <summary>
        /// Метод DeserializationMetric() используется для десериализации (а также её валидации) 
        /// данных для редактирования и добавления метрик в базу данных, отправлятся в виде:
        /// {"name":string,"isBoolean":bool,"warningThreshold":int,"alarmThreshold":int,"priority":enum(0,2),"kind":string}
        /// </summary>
        /// <param name="Data">Аргумент метода SerializationMetric()</param>
        public MetricItem DeserializationMetric(JsonElement Data)
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
        /// Метод StringValidator() является валидатором строк. Проверка входящих строк на длину строки и заполненость.
        /// </summary>
        /// <param name="Str">Строка, которую требуется проверить на корректность</param>
        /// <param name="Name">Наименование строки, которую требуется проверить на корректность</param>
        /// <returns>Возвращает строку с ошибкой, которая возникла при валидации</returns>
        public IEnumerable<string> StringValidator(string Str, string Name)
        {
            List<string> errors = new List<string>();
            const int MaxQuantity = 32;
            if (string.IsNullOrEmpty(Str))
            {
                logger.Error($"Поле {Name} должно быть заполнено!");
                errors.Add($"Поле {Name} должно быть заполнено!");
            }
            if (Str.Length > MaxQuantity)
            {
                logger.Error($"Длина строки {Name} должна быть  до 32 символов!");
                errors.Add($"Длина строки {Name} должна быть  до 32 символов!");
            }

            return errors;
        }
        /// <summary>
        /// Метод StringsValidation() используется для валидации вводимых строковых данных на корректность .
        /// С помощью валидатора StringValidator().
        /// </summary>
        /// <param name="parameters">Аргумент метода StringsValidation(). Принимает строки, которым нужно пройти валидацию.</param>
        /// <returns> Возвращает список строк ошибок возникших при валидации  </returns>
        public IEnumerable<string> StringsValidation(params string[] parameters)
        {       
            if (parameters.Length == 0)
            {
                return null;
            }
            List<string> names = new List<string> { "Name", "Kind" };
            List<string> errors = new List<string>();
            for (int i = 0; i < parameters.Length; i++)
            {
                errors.AddRange(StringValidator(parameters[i], names[i]).ToList());
            };
            return errors;
        }
        
    }
}
