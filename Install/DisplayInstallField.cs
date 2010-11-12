using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AppVisum.Sys.Install
{
    public abstract class DisplayInstallField : IInstallField
    {
        public abstract IHtmlString Render(HtmlHelper helper);

        public IEnumerable<KeyValuePair<string, string>> Parse(Dictionary<string, string> post, Dictionary<string, string> get)
        {
            yield break;
        }
    }
}
