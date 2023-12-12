using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;

namespace MVVM.Base
{
    [MarkupExtensionReturnType(typeof(BooleanConverter_ValueProvider))]
    public class BooleanConverter_ValueProvider : MarkupExtension
    {
        public virtual object TrueValue { get; set; } = true;
        public virtual object NullValue { get; set; } = false;
        public virtual object FalseValue { get; set; } = false;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class BooleanConverter : MarkupConverter
    {
        public virtual BooleanConverter_ValueProvider ValueProvider { get; set; }
        public BooleanConverter()
        {
                ValueProvider = new BooleanConverter_ValueProvider();
        }

        public virtual object TrueValue { get => ValueProvider.TrueValue; set => ValueProvider.TrueValue = value; }
        public virtual object NullValue { get => ValueProvider.NullValue; set => ValueProvider.NullValue = value; } 
        public virtual object FalseValue { get => ValueProvider.FalseValue; set => ValueProvider.FalseValue = value; } 

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return NullValue;

            if (value as bool? == true)
                return TrueValue;

            return FalseValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == NullValue)
                return null;

            if (value == TrueValue)
                return true;

            return false;
        }
    }
}