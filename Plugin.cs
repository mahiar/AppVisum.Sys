using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppVisum.Sys
{
    public class Plugin
    {
        private AppPluginStatus state;
        private IAppProvider plugin;
        private string instanceName;
        private Guid id;
        private IList<Configuration.PluginBinding> bindings;

        public Plugin()
        {
            bindings = new List<Configuration.PluginBinding>();
        }

        internal AppPluginStatus Status
        {
            get { return state; }
            set { state = value; }
        }

        public Guid Id
        {
            get { return id; }
            internal set { id = value; }
        }

        public IList<Configuration.PluginBinding> Bindings
        {
            get { return bindings; }
            internal set { bindings = value; }
        }

        public IAppProvider PluginObj
        {
            get { return plugin; }
            internal set { plugin = value; }
        }

        public String InstanceName
        {
            get { return instanceName; }
            internal set { instanceName = value; }
        }

        public int InstallStep
        {
            get
            {
                using (var session = AppSys.Instance.GetDBSession())
                {
                    return session.Load<Configuration.Plugin>(DbId).InstallStep;
                }
            }

            set
            {
                using (var session = AppSys.Instance.GetDBSession())
                {
                    var p = session.Load<Configuration.Plugin>(DbId);
                    p.InstallStep = value;
                    session.SaveChanges();
                }
            }
        }

        public bool IsInstanced
        {
            get { return (state & AppPluginStatus.Instance) == AppPluginStatus.Instance; }
        }

        public bool HasInstallWizzard
        {
            get { return (state & AppPluginStatus.Install) == AppPluginStatus.Install; }
        }

        public bool IsSetupComplete
        {
            get { return !((state & AppPluginStatus.SetupIncomplete) == AppPluginStatus.SetupIncomplete); }
        }

        public bool IsNew
        {
            get { return (state & AppPluginStatus.New) == AppPluginStatus.New; }
        }

        public bool IsEnabled
        {
            get { return ((state & AppPluginStatus.Enabled) == AppPluginStatus.Enabled) && IsSetupComplete; }
        }

        public bool IsModelProvider
        {
            get { return (state & AppPluginStatus.Model) == AppPluginStatus.Model; }
        }

        public bool IsContentProvider
        {
            get { return !IsModelProvider; }
        }

        public T P<T>()
        {
            return (T)PluginObj;
        }

        public string DbId
        {
            get
            {
                return GetPluginDbId(Id);
            }
        }

        public object StatusString
        {
            get
            {
                if (IsEnabled)
                    return "Enabled";
                else if (IsSetupComplete)
                    return "Disabled";
                else
                    return "Setup incomplete";
            }
        }

        public static string GetPluginDbId(Guid guid)
        {
            return "appvisum/appsys/plugin/" + guid.ToString();
        }

        public static Guid GetPluginId(string dbid)
        {
            return Guid.Parse(dbid.Replace("appvisum/appsys/plugin/", ""));
        }
    }
}
