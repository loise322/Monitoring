using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Validators;

namespace ApplicationCore.Validators
{
    class ThresholdsValidator : IValidators
    {
        protected override List<string> Validate(ValidationData data)
        {
            var errors = new List<string>();
            if (data.WarningThreshold >= data.AlarmThreshold)
            {
                errors.Add("Порог тревоги должен быть больше порога предупреждения");
                return errors;
            }
            return errors;
        }
    }
}
