using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AppVisum.Sys.ModelBinders
{
    public class IAppProviderModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext");

            Type type = bindingContext.ModelType;
            string id = GetA(typeof(String), bindingContext, bindingContext.ModelName) as string;

            if (id == null)
                return null;

            return AppSys.Instance.GetPluginById(Guid.Parse(id)).PluginObj;
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
