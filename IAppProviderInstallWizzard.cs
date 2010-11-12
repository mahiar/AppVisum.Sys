using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppVisum.Sys.Install;
using System.Web.Mvc;

namespace AppVisum.Sys
{
    public interface IAppProviderInstallWizzard : IAppProvider
    {
        int InstallationStepsCount { get; }
        //IEnumerable<IInstallField> GetStep(int step);
        IWizzardStep GetStep(int step);
        bool HandleStep(int step, object data, ModelStateDictionary modelState);
    }
}
