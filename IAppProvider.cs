using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppVisum.Sys
{
    [InheritedExport]
    [TypeConverter(typeof(TypeConverters.IAppProviderTypeConverter))]
    public interface IAppProvider
    {
        string Name { get; }
    }
}
