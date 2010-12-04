using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppVisum.Sys
{
    /// <summary>
    /// Public class for handeling plugins.
    /// </summary>
    public class Plugin
    {
        private AppPluginStatus state;
        private IAppProvider plugin;
        private string instanceName;
        private Guid id;
        private IList<Configuration.PluginBinding> bindings;

        internal Plugin()
        {
            bindings = new List<Configuration.PluginBinding>();
        }

        internal AppPluginStatus Status
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// The id of the plugin
        /// </summary>
        public Guid Id
        {
            get { return id; }
            internal set { id = value; }
        }

        /// <summary>
        /// Gets the bindings.
        /// </summary>
        /// <value>The bindings.</value>
        public IList<Configuration.PluginBinding> Bindings
        {
            get { return bindings; }
            internal set { bindings = value; }
        }

        /// <summary>
        /// Gets the plugin.
        /// </summary>
        /// <value>The plugin.</value>
        public IAppProvider PluginObj
        {
            get { return plugin; }
            internal set { plugin = value; }
        }

        /// <summary>
        /// Gets or name of the instance.
        /// </summary>
        /// <value>The name of the instance.</value>
        public String InstanceName
        {
            get { return instanceName; }
            internal set { instanceName = value; }
        }

        /// <summary>
        /// Gets or sets the current install step.
        /// </summary>
        /// <value>The current install step.</value>
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

        /// <summary>
        /// Gets a value indicating whether this plugin is instanced.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this plugin is instanced; otherwise, <c>false</c>.
        /// </value>
        public bool IsInstanced
        {
            get { return (state & AppPluginStatus.Instance) == AppPluginStatus.Instance; }
        }

        /// <summary>
        /// Gets a value indicating whether this plugin has an install wizzard.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this plugin has an install wizzard; otherwise, <c>false</c>.
        /// </value>
        public bool HasInstallWizzard
        {
            get { return (state & AppPluginStatus.Install) == AppPluginStatus.Install; }
        }

        /// <summary>
        /// Gets a value indicating whether this plugin's setup is complete.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this plugin's setup is complete; otherwise, <c>false</c>.
        /// </value>
        public bool IsSetupComplete
        {
            get { return !((state & AppPluginStatus.SetupIncomplete) == AppPluginStatus.SetupIncomplete); }
        }

        /// <summary>
        /// Gets a value indicating whether this plugin is new.
        /// </summary>
        /// <value><c>true</c> if this plugin is new; otherwise, <c>false</c>.</value>
        public bool IsNew
        {
            get { return (state & AppPluginStatus.New) == AppPluginStatus.New; }
        }

        /// <summary>
        /// Gets a value indicating whether this plugin is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this plugin is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get { return ((state & AppPluginStatus.Enabled) == AppPluginStatus.Enabled) && IsSetupComplete; }
        }

        /// <summary>
        /// Gets a value indicating whether this plugin is a model provider.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this plugin is a model provider; otherwise, <c>false</c>.
        /// </value>
        public bool IsModelProvider
        {
            get { return (state & AppPluginStatus.Model) == AppPluginStatus.Model; }
        }

        /// <summary>
        /// Gets a value indicating whether this plugin is  content provider.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this plugin is a content provider; otherwise, <c>false</c>.
        /// </value>
        public bool IsContentProvider
        {
            get { return (state & AppPluginStatus.Content) == AppPluginStatus.Content; }
        }

        /// <summary>
        /// Get's the plugin casted to T.
        /// </summary>
        /// <typeparam name="T">The type to cast the plugin to.</typeparam>
        /// <returns>The plugin casted to T.</returns>
        public T P<T>()
        {
            return (T)PluginObj;
        }

        /// <summary>
        /// Gets the db id.
        /// </summary>
        /// <value>The db id.</value>
        public string DbId
        {
            get
            {
                return GetPluginDbId(Id);
            }
        }

        /// <summary>
        /// Gets the status string.
        /// </summary>
        /// <value>The status string.</value>
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

        /// <summary>
        /// Gets the plugin's db id.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns>The db id</returns>
        public static string GetPluginDbId(Guid guid)
        {
            return "appvisum/appsys/plugin/" + guid.ToString();
        }

        /// <summary>
        /// Gets the plugin id.
        /// </summary>
        /// <param name="dbid">The db id.</param>
        /// <returns>The plugins id.</returns>
        public static Guid GetPluginId(string dbid)
        {
            return Guid.Parse(dbid.Replace("appvisum/appsys/plugin/", ""));
        }
    }
}
