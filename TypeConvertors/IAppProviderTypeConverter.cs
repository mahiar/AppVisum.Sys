using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AppVisum.Sys.TypeConverters
{
    public class IAppProviderTypeConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType.HasInterface(typeof(IAppProvider)))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null)
                return null;

            if (value is string)
            {
                string s = value as string;
                if (s.Length == 0)
                    return null;

                Guid id;
                if(Guid.TryParse(s, out id))
                    return AppSys.Instance.GetPluginById(id).PluginObj;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
                return String.Empty;

            if (value is IAppProvider)
            {
                return AppSys.Instance.GetPluginId((IAppProvider)value);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
