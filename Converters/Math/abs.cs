using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base.Common.Converters.Math
{
    public class abs : MarkupConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is float f)
                return System.Math.Abs(f);
            else if(value is double d)
                return System.Math.Abs(d);
            else if(value is decimal m)
                return System.Math.Abs(m);
            else if(value is long l)
                return System.Math.Abs(l);
            else if(value is int i)
                return System.Math.Abs(i);
            else if (value is short s)
                return System.Math.Abs(s);
            else if (value is sbyte sb)
                return System.Math.Abs(sb);

            return default;
        }
    }
}
