using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using System.Threading.Tasks;

namespace AppVisum.Sys
{
    public class AppVisumControllerFactory : DefaultControllerFactory
    {
        private Assembly assembly;
        private Dictionary<string, Type> controllersCache;

        public AppVisumControllerFactory(Assembly assembly)
        {
            // TODO: Complete member initialization
            this.assembly = assembly;
            InitCache();
        }

        private void InitCache()
        {
            this.controllersCache = new Dictionary<string, Type>();
            this.controllersCache.Add("AppVisumSetup", AppSys.Instance.SetupController.GetType());
            assembly.GetTypes().AsParallel()
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
                .Select(t => new { Name = t.Name.Substring(0, t.Name.Length - "Controller".Length), Type = t })
                .ForAll(t => controllersCache.Add(t.Name, t.Type));
        }

        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            string name = controllerName;
            string pluginId = "";
            if (requestContext.RouteData.Values.ContainsKey("plugin"))
            {
                pluginId = (string)requestContext.RouteData.Values["plugin"];
                name = pluginId + "/" + name;
            }
            if (controllersCache.ContainsKey(name))
                return controllersCache[name];
            if (requestContext.RouteData.Values.ContainsKey("plugin"))
            {
                Plugin plugin = AppSys.Instance.GetPluginById(pluginId);
                if (plugin.IsContentProvider)
                {
                    var controllerTypes = plugin.P<IAppContentProvider>().GetControllerTypes();
                    foreach (var ct in controllerTypes)
                    {
                        if (ct.FullName.Substring(0, ct.FullName.Length - "Controller".Length).Replace('.', '_').ToLower() == controllerName.ToLower())
                        {
                            controllersCache[name] = ct;
                            return ct;
                        }
                            
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
