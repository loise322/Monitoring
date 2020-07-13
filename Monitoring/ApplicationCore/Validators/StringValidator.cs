using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Models;
using NLog;

namespace ApplicationCore.Validators
{
    /// <summary>
    /// Валидатор для строковых данных на корректность.
    /// </summary>
    public class StringValidator : IStringValidator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Проверка входящих строк на длину строки и заполненность.
        /// </summary>
        /// <param name="stringForValidate">Строка, которую требуется проверить на корректность</param>
        /// <param name="name">Наименование строки, которую требуется проверить на корректность</param>
        /// <returns>Возвращает строку с ошибкой, которая возникла при валидации</returns>
        public List<string> ValidateMaxLength(string stringForValidate, string name)
        {
            var errors = new List<string>();
            const int maxQuantity = 32;
            if (string.IsNullOrEmpty(stringForValidate))
            {
                logger.Error($"Поле {name} должно быть заполнено!");
                errors.Add($"Поле {name} должно быть заполнено!");
                return errors;
            }
            if (!string.IsNullOrEmpty(stringForValidate) && stringForValidate.Length > maxQuantity)
            {
                logger.Error($"Длина строки {name} должна быть  до 32 символов!");
                errors.Add($"Длина строки {name} должна быть  до 32 символов!");
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
            var errors = new List<string>();
            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i].Kind == ValidationKind.MaxLength)
                {
                    errors.AddRange(ValidateMaxLength(data[i].Value, data[i].Name));
                }
            };
            return string.Join("\r\n", errors); ;
        }

        public List<ValidationData> SetValidationData(MetricItem data)
        {
            var validationData = new List<ValidationData>
            {
                new ValidationData { Name = "Name", Value = data.Name, Kind = ValidationKind.MaxLength },
                new ValidationData { Name = "Kind", Value = data.Kind, Kind = ValidationKind.MaxLength }
            };
            return validationData;
        }
    }
}
