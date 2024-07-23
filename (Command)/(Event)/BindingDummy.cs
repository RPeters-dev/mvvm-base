using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MVVM.Base
{

    public class BindingDummy : FrameworkElement, IDisposable
    {
        public static MethodInfo FrameworkElement_ChangeLogicalParent = typeof(FrameworkElement).GetMethod("ChangeLogicalParent", BindingFlags.NonPublic | BindingFlags.Instance);
        public static MethodInfo FrameworkElement_RemoveLogicalChild = typeof(FrameworkElement).GetMethod("RemoveLogicalChild", BindingFlags.NonPublic | BindingFlags.Instance);

        FrameworkElement bindingParent;

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(BindingDummy), new UIPropertyMetadata(null));

        public BindingDummy(FrameworkElement visualParent) : this(visualParent.DataContext, visualParent)
        {

        }

        public BindingDummy(object dataContext, FrameworkElement visualParent = null)
        {
            DataContext = dataContext;
            bindingParent = visualParent;

            //Give the dummy a new Parent. to Enshure Relative Bindings are working
            if (bindingParent != null)
                FrameworkElement_ChangeLogicalParent.Invoke(this, new[] { this.bindingParent });

        }

        public void Dispose()
        {
            if (bindingParent != null)
                FrameworkElement_RemoveLogicalChild.Invoke(this.bindingParent, new[] { this });
        }

        internal object GetValue(Binding binding)
        {
            BindingOperations.SetBinding(this, ValueProperty, binding);
            return GetValue(ValueProperty);
        }

        internal void SetValue(Binding binding, object value)
        {
            BindingOperations.SetBinding(this, ValueProperty, binding);
            SetValue(ValueProperty, value);
        }
    }
}
