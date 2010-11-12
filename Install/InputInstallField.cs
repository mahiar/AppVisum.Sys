using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AppVisum.Sys.Install
{
    public class InputInstallField<T> : IInstallField where T : TextBoxInstallField
    {
        private string title;
        private string name;
        private string value;
        private LableInstallField lable;
        private TextBoxInstallField textBox;

        public InputInstallField(string title, string name, string value)
        {
            this.title = title;
            this.name = name;
            this.value = value;

            lable = new LableInstallField(title, name);
            textBox = (TextBoxInstallField)Activator.CreateInstance(typeof(T), name, value);
        }

        public IHtmlString Render(HtmlHelper helper)
        {
            return new HtmlString(
                    String.Format("<div class=\"editor-label\">{0}</div><div class=\"editor-field\">{1}{2}</div>",
                        lable.Render(helper),
                        textBox.Render(helper),
                        helper.ValidationMessage(name, "*")
                    )
                );
        }

        public IEnumerable<KeyValuePair<string, string>> Parse(Dictionary<string, string> post, Dictionary<string, string> get)
        {
            foreach (var itm in textBox.Parse(post, get))
                yield return itm;

            yield break;
        }
    }
}
