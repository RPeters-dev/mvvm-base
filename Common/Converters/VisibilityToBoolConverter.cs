using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.Base.Common.Converters
{
    public class VisibilityToBoolConverter : MarkupConverter<Visibility>
    {
        public bool? VisibleResult { get; set; } = true;
        public bool? HiddenResult { get; set; } = false;
        public bool? CollapsedResult { get; set; } = false;



        public override object Convert(Visibility value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Visibility.Visible: return VisibleResult;
                case Visibility.Collapsed: return CollapsedResult;
                case Visibility.Hidden:return HiddenResult;
            }
            return CollapsedResult;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;

            if(boolValue == VisibleResult)
                return Visibility.Visible;
            else if (boolValue == CollapsedResult)
                return Visibility.Collapsed;
            else if(boolValue == HiddenResult)
                return Visibility.Hidden;

            return CollapsedResult;
        }

    }
}
