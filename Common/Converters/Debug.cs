using System;
using System.Globalization;

namespace MVVM.Base.Common.Converters
{
    public class Debug : MarkupConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
            return value;
        }
    }
}
