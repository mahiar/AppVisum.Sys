using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AppVisum.Sys
{
    public abstract class PluginController<T> : Controller where T : IAppContentProvider
    {
        Plugin plugin;
        T pluginObj;

        public PluginController(string pluginId)
            : base()
        {
            plugin = AppSys.Instance.GetPluginById(pluginId);
            pluginObj = plugin.P<T>();
        }

        public Plugin Plugin
        {
            get { return plugin; }
        }

        public T PluginObj
        {
            get { return pluginObj; }
        }
    }
}
