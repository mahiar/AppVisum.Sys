using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace AppVisum.Sys
{
    [InheritedExport]
    public interface IAppProvider
    {
        string Name { get; }
    }
}
