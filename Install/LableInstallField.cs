using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AppVisum.Sys.Install
{
    public class LableInstallField : DisplayInstallField
    {
        private string displayValue;
        private string fieldName;

        public LableInstallField(string displayValue, string fieldName)
        {
            this.displayValue = displayValue;
            this.fieldName = fieldName;
        }

        public override IHtmlString Render(HtmlHelper helper)
        {
            return (IHtmlString)helper.Label(displayValue, fieldName);
        }
    }
}
