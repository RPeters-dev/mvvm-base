using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace MVVM.Base
{
    public abstract class ContextMarkupCommand : MarkupCommand
    {
        static PropertyInfo FramesProperty;
        static FieldInfo _xamlContextField;
        static FieldInfo _stackField;
        static PropertyInfo PreviousFrameProperty;
        static PropertyInfo PreviousProperty;
        static PropertyInfo InstanceProperty;

        protected object TargetInstance { get; set; }
        protected object DataContext { get; set; }
        public BindingDummy Dummy { get; private set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (TargetInstance == null)
            {
                object FramesStack = null;
                if (serviceProvider.GetType().Name == "ServiceProviderWrapper")
                {
                    FramesStack = (FramesProperty = FramesProperty ?? serviceProvider.GetType().GetProperty("Frames", BindingFlags.NonPublic | BindingFlags.Instance))
                            .GetValue(serviceProvider);
                }
                else
                {
                    //serviceProvider.GetService
                    //_xamlContext = { MS.Internal.Xaml.Context.ObjectWriterContext}
                    var _xamlContext = (_xamlContextField = _xamlContextField ?? serviceProvider.GetType().GetField("_xamlContext", BindingFlags.NonPublic | BindingFlags.Instance))
                        .GetValue(serviceProvider);

                    //_stack = { MS.Internal.Xaml.Context.XamlContextStack<MS.Internal.Xaml.Context.ObjectWriterFrame>}
                    FramesStack = (_stackField = _stackField ?? _xamlContext.GetType().GetField("_stack", BindingFlags.NonPublic | BindingFlags.Instance))
                       .GetValue(_xamlContext);
                }
                // PreviousFrame = "Binding._PositionalParameters inst=- coll=*"
                var PreviousFrame = (PreviousFrameProperty = PreviousFrameProperty ?? FramesStack.GetType().GetProperty("PreviousFrame", BindingFlags.Public | BindingFlags.Instance))
                   .GetValue(FramesStack);

                PreviousProperty = PreviousProperty ?? PreviousFrame.GetType().GetProperty("Previous", BindingFlags.Public | BindingFlags.Instance);
                InstanceProperty = InstanceProperty ?? PreviousFrame.GetType().GetProperty("Instance", BindingFlags.Public | BindingFlags.Instance);

                TargetInstance = null;
                while (TargetInstance == null || PreviousFrame == null)
                {
                    TargetInstance = InstanceProperty.GetValue(PreviousFrame);

                    if (TargetInstance == null)
                    {
                        PreviousFrame = PreviousProperty.GetValue(PreviousFrame);
                    }
                }

            }

            if (TargetInstance is FrameworkElement fe)
            {
                DataContext = fe.DataContext;
                Dummy = new BindingDummy(fe);

            }
            return base.ProvideValue(serviceProvider);
        }

        public object GetValue(Binding b)
        {
            if (b == null || TargetInstance == null)
                return null;

            if (TargetInstance is FrameworkElement fe)
            {
                return Dummy.GetValue(b);
            }
            return default;
        }

        public void SetValue(Binding b, object value)
        {
            if (b == null || TargetInstance == null)
                return;

            if (TargetInstance is FrameworkElement fe)
            {
                Dummy.SetValue(b, value);
            }
        }

        public T GetValue<T>(object o)
        {
            if (o == null || TargetInstance == null)
                return default;

            if (o is Binding b)
            {
                return (T)GetValue(b);
            }

            return (T)o;
        }
    }
}
