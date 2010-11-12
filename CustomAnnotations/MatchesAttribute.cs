using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppVisum.Sys.CustomAnnotations
{
    public class MatchesAttribute : ValidationAttribute
    {
        private string source;

        public MatchesAttribute(string source)
        {
            this.source = source;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var prop = validationContext.ObjectType.GetProperty(source);
            if (prop == null)
                throw new ArgumentNullException("source", "No property by that name defined.");

            object sVal = prop.GetValue(validationContext.ObjectInstance, null);

            if ((sVal == null && value == null) || (sVal != null && sVal.Equals(value)))
                return ValidationResult.Success;
            else
                return new ValidationResult(String.Format("The {0} field did not match the {1} field.", validationContext.DisplayName, source), new string[] { validationContext.MemberName, source });
        }
    }

}
