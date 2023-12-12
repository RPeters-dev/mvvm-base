using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base
{
    public class IsNullOrWhiteSpaceConverter : BooleanConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return TrueValue;

            if (String.IsNullOrWhiteSpace(value.ToString()))
                return TrueValue;


            return FalseValue;
        }
    }
}
