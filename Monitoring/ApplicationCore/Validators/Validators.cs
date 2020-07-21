using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models;
using ApplicationCore.Validators;

namespace ApplicationCore.Validators
{
    public class Validators
    {
        public string ValidateAll(List<ValidationData> datalist)
        {
            IValidators[] validationTypes = SetValidationTypes();
            List<string> errors = new List<string>();
            byte i = 0;
            foreach (var item in validationTypes)
            {
                errors.AddRange(item.ValidateOnce(datalist[i]));
                i += 1;
            }
            return String.Join("\r\n", errors);
        }

        public List<ValidationData> SetValidationData(MetricItem data)
        {
            var validationData = new List<ValidationData>
            {
                new ValidationData { Name = "Name", Value = data.Name, Kind = ValidationKind.MaxLength },
                new ValidationData { Name = "Kind", Value = data.Kind, Kind = ValidationKind.MaxLength },
                new ValidationData { WarningThreshold = data.WarningThreshold, AlarmThreshold = data.AlarmThreshold, Kind = ValidationKind.Thresholds }
            };
            return validationData;
        }

        public IValidators[] SetValidationTypes()
        {
            var validationTypes = new IValidators[]
            {
                new MaxLengthValidator(), 
                new MaxLengthValidator(),
                new ThresholdsValidator()
            };
            return validationTypes;
        }
    }
}
