using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppVisum.Sys
{
    public class PluginSettingsProvider
    {
        AppSys sys;

        internal PluginSettingsProvider(AppSys sys)
        {
            this.sys = sys;
        }

        public string Get(IAppProvider plugin, string key)
        {
            Plugin pl = sys.InstalledPlugins.First(p => Object.ReferenceEquals(p.PluginObj, plugin));

            using (var session = sys.GetDBSession())
            {
                Configuration.Plugin pConf = session.Load<Configuration.Plugin>(pl.DbId);
                if (!pConf.Settings.ContainsKey(key))
                    return null;
                return pConf.Settings[key];
            }
        }

        public void Set(IAppProvider plugin, string key, string value)
        {
            Plugin pl = sys.InstalledPlugins.First(p => Object.ReferenceEquals(p.PluginObj, plugin));

            using (var session = sys.GetDBSession())
            {
                Configuration.Plugin pConf = session.Load<Configuration.Plugin>(pl.DbId);
                pConf.Settings[key] = value;
                session.SaveChanges();
            }
        }

        public void Set(IAppProvider plugin, string key, object value)
        {
            Set(plugin, key, value.ToString());
        }
    }
}
