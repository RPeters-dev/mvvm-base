using System;
using System.Globalization;

namespace MVVM.Base
{
    public class BooleanConverter : MarkupConverter
    {
        public virtual object TrueValue { get; set; } = true;
        public virtual object NullValue { get; set; } = false;
        public virtual object FalseValue { get; set; } = false;

        public override object ConvertOverride(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return NullValue;

            if (value as bool? == true)
                return TrueValue;

            return FalseValue;
        }

        public override object ConvertBackOverride(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == NullValue)
                return null;

            if (value == TrueValue)
                return true;

            return false;
        }
    }
}