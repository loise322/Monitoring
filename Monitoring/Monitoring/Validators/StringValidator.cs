using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Monitoring.Models;
using Monitoring.ViewModels;
using Newtonsoft.Json;
using NLog;

namespace Monitoring.Validators
{
    /// <summary>
    /// Валидатор для строковых данных на корректность.
    /// </summary>
    public class StringValidator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Проверка входящих строк на длину строки и заполненость.
        /// </summary>
        /// <param name="StringForValidate">Строка, которую требуется проверить на корректность</param>
        /// <param name="Name">Наименование строки, которую требуется проверить на корректность</param>
        /// <returns>Возвращает строку с ошибкой, которая возникла при валидации</returns>
        public List<string> ValidateMaxLength(string StringForValidate, string Name)
        {
            List<string> errors = new List<string>();
            const int MaxQuantity = 32;
            if (string.IsNullOrEmpty(StringForValidate))
            {
                logger.Error($"Поле {Name} должно быть заполнено!");
                errors.Add($"Поле {Name} должно быть заполнено!");
                return errors;
            }
            if (!string.IsNullOrEmpty(StringForValidate) && StringForValidate.Length > MaxQuantity)
            {
                logger.Error($"Длина строки {Name} должна быть  до 32 символов!");
                errors.Add($"Длина строки {Name} должна быть  до 32 символов!");
            }
            return errors;
        }

        /// <summary>
        /// Валидация вводимых строковых данных на корректность через ValidateMaxLength().
        /// </summary>
        /// <param name="data"> Принимает строки, которым нужно пройти валидацию.</param>
        /// <returns> Возвращает ошибки возникшие при валидации</returns>
        public string ValidateStrings(List<ValidationData> data)
        {       
            if (data.Count() == 0)
            {
                return null;
            }
            List<string> errors = new List<string>();
            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i].Kind == ValidationKind.MaxLength)
                {
                    errors.AddRange(ValidateMaxLength(data[i].Value, data[i].Name));
                }
            };
            return string.Join("\r\n", errors); ;
        }
    }
}
