using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AppVisum.Sys.Razor
{
    public static class ApplicationPartRegistry
    {
        static ApplicationPartRegistry()
        {
            Instance = new DictionaryBasedApplicationPartRegistry();
        }
        public static IApplicationPartRegistry Instance { get; private set; }

        public static Type GetCompiledType(string virtualPath)
        {
            return Instance.GetCompiledType(virtualPath);
        }

        public static void Register(Assembly applicationPart)
        {
            Register(applicationPart, null);
        }

        public static void Register(Assembly applicationPart, string rootVirtualPath)
        {
            Instance.Register(applicationPart, rootVirtualPath);
        }

        public static void RegisterWebPage(Type type)
        {
            RegisterWebPage(type, null);
        }
        public static void RegisterWebPage(Type type, string rootVirtualPath)
        {
            Instance.RegisterWebPage(type, rootVirtualPath);
        }
    }
}
