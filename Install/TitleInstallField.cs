using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AppVisum.Sys.Install
{
    public class TitleInstallField : DisplayInstallField
    {
        private string title;
        private int titleType;

        public TitleInstallField(string title, int titleType = 3)
        {
            this.title = title;
            this.titleType = titleType;
        }

        public override IHtmlString Render(HtmlHelper helper)
        {
            return new HtmlString(String.Format("<h{1}>{0}</h{1}>", title, titleType));
        }
    }
}
