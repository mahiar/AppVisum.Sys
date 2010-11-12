using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AppVisum.Sys.Install
{
    public interface IInstallField
    {
        IHtmlString Render(HtmlHelper html);
        IEnumerable<KeyValuePair<String, String>> Parse(Dictionary<String, String> post, Dictionary<String, String> get);
    }
}
