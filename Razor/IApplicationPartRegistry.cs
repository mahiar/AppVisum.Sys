using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AppVisum.Sys.Razor
{
    public interface IApplicationPartRegistry
    {
        Type GetCompiledType(string virtualPath);
        void Register(Assembly applicationPart);
        void Register(Assembly applicationPart, string rootVirtualPath);
        void RegisterWebPage(Type type);
        void RegisterWebPage(Type type, string rootVirtualPath);
    }
}
