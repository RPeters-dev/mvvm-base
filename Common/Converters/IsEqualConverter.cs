using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base.Common.Converters
{
    public class IsEqualConverter : BooleanConverter
    {
        public object EqualValue { get; set; }

        public override object ConvertOverride(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && EqualValue == null)
                return TrueValue;

            if (value == null)
                return FalseValue;

            return value.Equals(EqualValue) ? TrueValue : FalseValue;
        }
    }
}
