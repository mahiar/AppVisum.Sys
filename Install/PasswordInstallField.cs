using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppVisum.Sys.Install
{
    public class PasswordInstallField : TextBoxInstallField
    {
        public PasswordInstallField(string name, string value = "")
            : base(name, value)
        {
            this.type = "password";
        }
    }
}
