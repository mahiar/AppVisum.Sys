using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AppVisum.Sys
{
    public class WizzardStepModelBinder : DefaultModelBinder
    {
        public WizzardStepModelBinder()
        {
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext");

            Plugin plugin = AppSys.Instance.GetPluginById((Guid)GetA(typeof(Guid), bindingContext, "id"));
            Type wizType = plugin.P<IAppProviderInstallWizzard>().GetStep(plugin.InstallStep).GetType();

            foreach (var prop in wizType.GetProperties())
                bindingContext.PropertyMetadata[prop.Name] = ModelMetadataProviders.Current.GetMetadataForProperty(null, wizType, prop.Name);

            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, wizType);

            object ret = base.BindModel(controllerContext, bindingContext);

            return ret;
        }

        private object GetA(Type type, ModelBindingContext context, string key)
        {
            if (String.IsNullOrEmpty(key))
                key = context.ModelName;

            ValueProviderResult valueResult = context.ValueProvider.GetValue(context.ModelName + "." + key);

            if (valueResult == null && context.FallbackToEmptyPrefix)
                valueResult = context.ValueProvider.GetValue(key);

            if (valueResult == null)
                return null;

            return valueResult.ConvertTo(type);
        }
    }
}
