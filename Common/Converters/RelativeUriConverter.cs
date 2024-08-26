using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MVVM.Base.Common.Converters
{
    public class RelativeUriConverter : MarkupConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Uri)
            {
                return value;
            }

            return new Uri(value.ToString(), UriKind.Relative);
        }
    }
}
