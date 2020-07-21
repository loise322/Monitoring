using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Validators;
using NLog;

namespace ApplicationCore.Validators
{
    class MaxLengthValidator : IValidators
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected override List<string> Validate(ValidationData data)
        {
            var errors = new List<string>();
            const int maxQuantity = 32;
            if (string.IsNullOrEmpty(data.Value))
            {
                logger.Error($"Поле {data.Name} должно быть заполнено!");
                errors.Add($"Поле {data.Name} должно быть заполнено!");
                return errors;
            }
            if (!string.IsNullOrEmpty(data.Value) && data.Value.Length > maxQuantity)
            {
                logger.Error($"Длина строки {data.Name} должна быть  до 32 символов!");
                errors.Add($"Длина строки {data.Name} должна быть  до 32 символов!");
            }
            return errors;
        }
    }
}
