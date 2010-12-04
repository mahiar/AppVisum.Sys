using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AppVisum.Sys.CustomAnnotations
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(@"^([a-zA-Z0-9_\-\+]+\.?)+[a-zA-Z0-9]@((([0-9]{1,3}\.){3}[0-9]{1,3})|(([a-zA-Z0-9_\-]+\.)+[a-zA-Z]{2,3}))$")
        {
            this.ErrorMessage = "Source was not an email.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
                value = value.ToString().Trim();
            return base.IsValid(value, validationContext);
        }
    }
}
