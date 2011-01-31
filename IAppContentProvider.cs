using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace AppVisum.Sys
{
    public interface IAppContentProvider : IAppProvider
    {
        IEnumerable<Type> GetControllerTypes();
        IEnumerable<Type> GetViewTypes();
    }
}
