using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppVisum.Sys
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            if (requestContext.RouteData.Values.ContainsKey("plugin"))
            {
                string pluginId = (string)requestContext.RouteData.Values["plugin"];
                Plugin plugin = AppSys.Instance.GetPluginById(pluginId);
                if (plugin.IsContentProvider)
                {
                    var controllerTypes = plugin.P<IAppContentProvider>().GetControllerTypes();
                    foreach (var ct in controllerTypes)
                    {
                        if (ct.Name.Substring(0, ct.Name.Length - "Controller".Length).ToLower() == controllerName.ToLower())
                            return ct;
                    }
                }
            }


            return base.GetControllerType(requestContext, controllerName);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (requestContext.RouteData.Values.ContainsKey("plugin"))
            {
                string pluginId = (string)requestContext.RouteData.Values["plugin"];

                try { return (IController)controllerType.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { pluginId }); }
                catch { }
            }
            return base.GetControllerInstance(requestContext, controllerType);
        }
    }
}
