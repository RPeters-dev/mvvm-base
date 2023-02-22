using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MVVM.Base
{
    public abstract class MarkupConverter : MarkupExtension, IValueConverter
    {
        #region Methods

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion Methods
    }
}