using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MVVM.Base.Common.Converters
{ 
    public abstract class MarkupConverter<T> : MarkupConverter
    {
        public abstract object Convert(T value, Type targetType, object parameter, CultureInfo culture);


        public override object ConvertOverride(object value, Type targetType, object parameter, CultureInfo culture)
        {
              return Convert(value is T ? (T)value : default(T), targetType, parameter, culture);
        }
    }

    public delegate object IValueConvertResolve(MarkupConverter source, object value, Type targetType, object parameter, CultureInfo culture);

    public abstract class MarkupConverter : MarkupExtension, IValueConverter
    {
        #region Methods

        public IValueConvertResolve Convert { get; set; } = DefaultConvert;
        public IValueConvertResolve ConvertBack { get; set; } = DefaultConvertBack;

        private static object DefaultConvert(MarkupConverter source, object value, Type targetType, object parameter, CultureInfo culture)
        {
            return source.ConvertOverride(value, targetType, parameter, culture);
        }
        private static object DefaultConvertBack(MarkupConverter source, object value, Type targetType, object parameter, CultureInfo culture)
        { 
            return source.ConvertOverride(value, targetType, parameter, culture);
        }

        public virtual object ConvertOverride(object value, Type targetType, object parameter, CultureInfo culture)
        { return value; }

        public virtual object ConvertBackOverride(object value, Type targetType, object parameter, CultureInfo culture)
        { return value; }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) => Convert(this, value, targetType, parameter, culture);
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertBack(this, value, targetType, parameter, culture);

        #endregion Methods
    }
}