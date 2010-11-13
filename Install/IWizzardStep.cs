using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AppVisum.Sys.Install
{
    public interface IWizzardStep
    {
        string GetTitle();
        void SetAppProvider(IAppProviderInstallWizzard appProvider);
    }
}
