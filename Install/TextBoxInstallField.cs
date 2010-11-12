using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AppVisum.Sys.Install
{
    public class TextBoxInstallField : IInstallField
    {
        private string name;
        private string value;
        protected string type = "text";

        public TextBoxInstallField(string name, string value = "")
        {
            this.name = name;
            this.value = value;
        }

        public IHtmlString Render(HtmlHelper helper)
        {
            return type == "text" ? (IHtmlString)helper.TextBox(name, value) : (IHtmlString)helper.Password(name, value);
        }

        public IEnumerable<KeyValuePair<string, string>> Parse(Dictionary<string, string> post, Dictionary<string, string> get)
        {
            if (post.ContainsKey(name))
                yield return new KeyValuePair<string, string>(name, post[name]);

            yield break;
        }
    }
}
