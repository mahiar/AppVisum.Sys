using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;

namespace AppVisum.Sys.Razor
{
    public class Rendrer<T, T2> : IView where T : System.Web.Mvc.WebViewPage<T2>
    {
        public Rendrer()
        {

        }

        public string Render(T2 model)
        {
            return Render(new ViewDataDictionary<T2>(model));
        }

        public string Render(ViewDataDictionary<T2> viewData)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            ControllerContext cc = new ControllerContext();
            cc.Controller = null;
            cc.HttpContext = new HttpContextWrapper(HttpContext.Current);

            ViewContext vc = new ViewContext(cc, this, new ViewDataDictionary<T2>(viewData), new TempDataDictionary(), writer);


            Render(vc, writer);

            return sb.ToString();
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            T page = Activator.CreateInstance<T>();
            page.VirtualPath = "";
            page.ViewContext = viewContext;
            page.ViewData = (ViewDataDictionary<T2>)viewContext.ViewData;
            page.InitHelpers();

            var httpContext = viewContext.HttpContext;
            WebPageRenderingBase base4 = null;
            object m = null;

            page.ExecutePageHierarchy(new WebPageContext(httpContext, base4, m), writer);
        }
    }
}
