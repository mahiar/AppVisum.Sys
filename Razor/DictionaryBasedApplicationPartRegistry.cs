using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.WebPages;
using System.Reflection;
using System.Web;

namespace AppVisum.Sys.Razor
{
    public class DictionaryBasedApplicationPartRegistry : IApplicationPartRegistry
    {
        private static readonly Type webPageType = typeof(WebPageRenderingBase);
        private readonly Dictionary<string, Type> registeredPaths = new Dictionary<string, Type>();

        public virtual Type GetCompiledType(string virtualPath)
        {
            if (virtualPath == null) throw new ArgumentNullException("virtualPath");

            if (!virtualPath.StartsWith("~/"))
                virtualPath = !virtualPath.StartsWith("/") ? "~/" + virtualPath : "~" + virtualPath;

            virtualPath = virtualPath.ToLower();
            return registeredPaths.ContainsKey(virtualPath) ? registeredPaths[virtualPath] : null;
        }

        public void Register(Assembly applicationPart)
        {
            this.Register(applicationPart, null);
        }

        public virtual void Register(Assembly applicationPart, string virtualRootPath)
        {
            foreach (var type in applicationPart.GetTypes().Where(type => type.IsSubclassOf(webPageType)))
                this.RegisterWebPage(type, virtualRootPath);
        }

        public void RegisterWebPage(Type type)
        {
            this.RegisterWebPage(type, null);
        }

        public void RegisterWebPage(Type type, string virtualRootPath)
        {
            if (!type.IsSubclassOf(webPageType)) throw new ArgumentException("type is not a subclass of WebPageRenderingBase.");
            var attribute = type.GetCustomAttributes(typeof(PageVirtualPathAttribute), false).Cast<PageVirtualPathAttribute>().SingleOrDefault();
            if (attribute != null)
            {
                var rootRelativeVirtualPath = GetRootRelativeVirtualPath(virtualRootPath ?? "", attribute.VirtualPath);
                registeredPaths[rootRelativeVirtualPath.ToLower()] = type;
            }
        }

        static string GetRootRelativeVirtualPath(string rootVirtualPath, string pageVirtualPath)
        {
            string relativePath = pageVirtualPath;
            if (relativePath.StartsWith("~/", StringComparison.Ordinal))
                relativePath = relativePath.Substring(2);

            if (!rootVirtualPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                rootVirtualPath = rootVirtualPath + "/";

            relativePath = VirtualPathUtility.Combine(rootVirtualPath, relativePath);
            if (!relativePath.StartsWith("~"))
                return !relativePath.StartsWith("/") ? "~/" + relativePath : "~" + relativePath;

            return relativePath;
        }
    }
}
