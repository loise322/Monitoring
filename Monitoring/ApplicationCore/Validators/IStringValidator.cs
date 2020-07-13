using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models;

namespace ApplicationCore.Validators
{
    public interface IStringValidator
    {
        string ValidateStrings(List<ValidationData> data);

        List<ValidationData> SetValidationData(MetricItem data);
    }
}
