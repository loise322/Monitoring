using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Validators;

namespace ApplicationCore.Validators
{
    public abstract class IValidators
    {
        public List<string> ValidateOnce(ValidationData data)
        {
            var errors = new List<string>();
            errors.AddRange(Validate(data));
            return errors;
        }


        protected abstract List<string> Validate(ValidationData data);
    }
}
