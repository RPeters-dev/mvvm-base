using System;
using System.Windows;
using System.Windows.Data;

namespace MVVM.Base
{

    public class BindingDummy : FrameworkElement
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(BindingDummy), new UIPropertyMetadata(null));

        public BindingDummy(object dataContext)
        {
            DataContext=dataContext;
        }

        internal object GetValue(Binding binding)
        {
            BindingOperations.SetBinding(this, ValueProperty, binding);
            return GetValue(ValueProperty);
        }
    }
}
