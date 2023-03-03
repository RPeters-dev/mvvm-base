using System;
using System.Windows;
using System.Windows.Data;

namespace MVVM.Base
{
    public static class EventBindings
    {
        // Using a DependencyProperty as the backing store for EventBinders.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventBindersProperty =
            DependencyProperty.RegisterAttached("EventBinders", typeof(EventBinders), typeof(EventBindings), new PropertyMetadata(null, OnEventBindersChanged));

        // Using a DependencyProperty as the backing store for EventCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventCommandProperty =
            DependencyProperty.RegisterAttached("EventCommand", typeof(Command), typeof(EventBindings), new PropertyMetadata(null, OnEventChanged));

        public static readonly DependencyProperty EventObjectProperty =
        DependencyProperty.RegisterAttached("EventObject", typeof(EventBinding), typeof(EventBindings), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for EventParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventParameterProperty =
            DependencyProperty.RegisterAttached("EventParameter", typeof(object), typeof(EventBindings), new PropertyMetadata(null, OnEventChanged));

        public static readonly DependencyProperty EventProperty =
            DependencyProperty.RegisterAttached("Event", typeof(string), typeof(EventBindings), new PropertyMetadata(null, OnEventChanged));

        public static string GetEvent(DependencyObject obj)
        {
            return (string)obj.GetValue(EventProperty);
        }

        public static EventBinders GetEventBinders(DependencyObject obj)
        {
            return (EventBinders)obj.GetValue(EventBindersProperty);
        }

        public static EventBinding GetEventObject(DependencyObject obj)
        {
            return (EventBinding)obj.GetValue(EventObjectProperty);
        }

        public static Command GetEventCommand(DependencyObject obj)
        {
            return (Command)obj.GetValue(EventCommandProperty);
        }

        public static Object GetEventParameter(DependencyObject obj)
        {
            return (Object)obj.GetValue(EventParameterProperty);
        }
        public static void SetEvent(DependencyObject obj, string value)
        {
            obj.SetValue(EventProperty, value);
        }

        public static void SetEventBinders(DependencyObject obj, EventBinders value)
        {
            obj.SetValue(EventBindersProperty, value);
        }
        public static void SetEventCommand(DependencyObject obj, Command value)
        {
            obj.SetValue(EventCommandProperty, value);
        }

        public static void SetEventParameter(DependencyObject obj, Object value)
        {
            obj.SetValue(EventParameterProperty, value);
        }
        private static void OnEventBindersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnEventBindersChanged(d, (EventBinders)e.OldValue, (EventBinders)e.NewValue);
        }

        private static void OnEventBindersChanged(DependencyObject d, EventBinders oldValue, EventBinders newValue)
        {
            oldValue?.RemoveBindings(d);
            newValue?.AddBindings(d);
        }

        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var eventName = GetEvent(d);

            var binding = (EventBinding)d.GetValue(EventObjectProperty);
            if (binding != null)
            {
                if (binding.EventName != eventName && binding.IsAttached)
                    binding.Detach(d);
            }
            else
            {
                binding = new EventBinding();
            }

            binding.EventName = eventName;
            binding.Command = BindingOperations.GetBinding(d, EventCommandProperty);
            binding.CommandParameter = BindingOperations.GetBinding(d, EventParameterProperty);

            if (binding.EventName != null && !binding.IsAttached)
                binding.Attach(d);

            d.SetValue(EventObjectProperty, binding);

        }

    }
}
