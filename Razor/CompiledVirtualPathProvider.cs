using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace AppVisum.Sys.Razor
{
    public class CompiledVirtualPathProvider : VirtualPathProvider
    {
        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <returns>
        /// true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        /// <param name="virtualPath">The path to the virtual file.</param>
        public override bool FileExists(string virtualPath)
        {
            var ret = GetCompiledType(virtualPath) != null
                || Previous.FileExists(virtualPath);
            return ret;
        }

        private Type GetCompiledType(string virtualPath)
        {
            var ret = ApplicationPartRegistry.Instance.GetCompiledType(virtualPath);
            return ret;
        }

        /// <summary>
        /// Gets a virtual file from the virtual file system.
        /// </summary>
        /// <returns>
        /// A descendent of the <see cref="T:System.Web.Hosting.VirtualFile"/> class that represents a file in the virtual file system.
        /// </returns>
        /// <param name="virtualPath">The path to the virtual file.</param>
        public override VirtualFile GetFile(string virtualPath)
        {
            VirtualFile ret;
            if (Previous.FileExists(virtualPath))
            {
                ret = Previous.GetFile(virtualPath);
                return ret;
            }
            var compiledType = GetCompiledType(virtualPath);
            if (compiledType != null)
            {
                ret = new CompiledVirtualFile(virtualPath, compiledType);
                return ret;
            }
            ret = null;
            return ret;
        }

        public override string GetFileHash(string virtualPath, System.Collections.IEnumerable virtualPathDependencies)
        {
            if (Previous.FileExists(virtualPath))
            {
                return Previous.GetFileHash(virtualPath, virtualPathDependencies);
            }
            var compiledType = GetCompiledType(virtualPath);
            if (compiledType != null)
            {
                //for some reason, caching of our custom build result fails
                //making this a unique value will cause our custom buildprovider
                //always. Shouldn't be too bad, since it doesn't have to compile
                //anyway.
                return DateTime.Now.Ticks + "";
                //return compiledType.Assembly.Location.GetHashCode() + "";
            }
            return base.GetFileHash(virtualPath, virtualPathDependencies);
        }

        public override string GetCacheKey(string virtualPath)
        {
            return base.GetCacheKey(virtualPath);
        }

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (virtualPathDependencies == null)
                return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

            return Previous.GetCacheDependency(virtualPath, 
                    from vp in virtualPathDependencies.Cast<string>()
                    where GetCompiledType(vp) == null
                    select vp
                  , utcStart);
        }

    }
}
