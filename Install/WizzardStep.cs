using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppVisum.Sys.Install
{
    public abstract class WizzardStep : IWizzardStep
    {
        protected IAppProviderInstallWizzard appProvider = null;
        public void SetAppProvider(IAppProviderInstallWizzard appProvider)
        {
            this.appProvider = appProvider;
        }

        public abstract string GetTitle();
    }
}
