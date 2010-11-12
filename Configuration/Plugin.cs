using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppVisum.Sys.Configuration
{
    public enum PluginStatus
    {
        Enabled,
        Disabled
    }

    public class Plugin
    {
        public Plugin()
        {
            Id = AppVisum.Sys.Plugin.GetPluginDbId(Guid.NewGuid());
            Bindings = new List<PluginBinding>();
            Settings = new Dictionary<string, string>();
        }

        public int InstallStep { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public PluginStatus Status { get; set; }

        public string PluginTypeName { get; set; }

        public string InstanceName { get; set; }

        public Dictionary<string, string> Settings { get; set; }

        public IList<PluginBinding> Bindings { get; set; }
    }
}
