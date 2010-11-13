using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace AppVisum.Sys.CustomAnnotations
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class FunctionPickAttribute : Attribute
    {
        private string func;
        public FunctionPickAttribute(string func)
        {
            this.func = func;
        }

        public string GetFunc()
        {
            return func;
        }
    }
}
