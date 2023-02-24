using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

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

    /// <summary>
    /// Provides the base class for a collection of <see cref="EventBinding"/>.
    /// </summary>
    public class EventBinders : Collection<EventBinding>
    {
        internal void AddBindings(DependencyObject d)
        {
            foreach (var item in this)
            {
                item.Attach(d);
            }
        }

        internal void RemoveBindings(DependencyObject d)
        {
            foreach (var item in this)
            {
                item.Detach(d);
            }
        }
    }

    public class EventBinding
    {
        static Dictionary<string, Delegate> cache = new Dictionary<string, Delegate>();
        /// <summary>
        /// The taget Command
        /// </summary>
        public Binding Command { get; set; }

        /// <summary>
        /// The taget Command
        /// </summary>
        public Binding CommandParameter { get; set; }

        /// <summary>
        /// Name of the Event that executes the <see cref="Command"/>
        /// </summary>
        public string EventName { get; set; }
        public bool IsAttached { get; private set; }

        internal void Attach(DependencyObject d)
        {
            var objectType = d.GetType();
            var targetEvent = objectType.GetEvent(EventName);

            if (targetEvent == null)
            {
                Debugger.Log(0, "Error", $"{typeof(EventBinder)} Error: can't find Event '{EventName}' on '{d.GetType()}'");
                return;
            }

            var cacheKey = targetEvent.ToString();

            if (!cache.TryGetValue(cacheKey, out var del))
            {
                var invoker = targetEvent.EventHandlerType.GetMethod(nameof(EventHandler.Invoke));
                var invokerParameter = invoker.GetParameters();

                var eventHandler = new DynamicMethod($"dyn_{EventName}", invoker.ReturnType, invokerParameter.Select(x => x.ParameterType).ToArray());

                var il = eventHandler.GetILGenerator();
                il.Emit(OpCodes.Ldc_I4, invokerParameter.Length + 1);
                il.Emit(OpCodes.Newarr, typeof(object));
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ldstr, EventName);
                il.Emit(OpCodes.Stelem_Ref);

                for (int i = 0; i < invokerParameter.Length; i++)
                {
                    eventHandler.DefineParameter(i, ParameterAttributes.In, invokerParameter[i].Name);
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Ldc_I4, i + 1);
                    il.Emit(OpCodes.Ldarg, i);
                    il.Emit(OpCodes.Stelem_Ref);
                }

                il.Emit(OpCodes.Call, typeof(EventBinding).GetMethod(nameof(EventCall), BindingFlags.Static | BindingFlags.NonPublic));
                il.Emit(OpCodes.Ret);

                del = eventHandler.CreateDelegate(targetEvent.EventHandlerType);
                cache.Add(cacheKey, del);
            }

            targetEvent.AddEventHandler(d, del);
            IsAttached = true;
        }

        internal void Detach(DependencyObject d)
        {
            var objectType = d.GetType();
            var targetEvent = objectType.GetEvent(EventName);
            var cacheKey = targetEvent.ToString();

            if (cache.TryGetValue(cacheKey, out var del))
            {
                targetEvent.RemoveEventHandler(d, del);
            }
            IsAttached = false;

        }

        private static void EventCall(object[] parameter)
        {
            var eventName = parameter[0].ToString();

            FrameworkElement sender = null;
            if (parameter.Length < 1 || (sender = (FrameworkElement)parameter[1]) == null)
            {
                Debugger.Log(0, "Error", $"{typeof(EventBinder)} argument error: first argument of '{eventName}' must be a FrameworkElement");
                return;
            }

            var eventBinders = EventBindings.GetEventBinders(sender)?.Where(x => x.EventName.Equals(eventName));
            if (eventBinders == null)
            {
                eventBinders = new[] { EventBindings.GetEventObject(sender) };
            }

            var dummy = new EventBinder();
            dummy.DataContext = sender.DataContext;

            foreach (var item in eventBinders)
            {
                BindingOperations.SetBinding(dummy, EventBinder.ValueProperty, item.Command);
                var targetCommand = dummy.GetValue(EventBinder.ValueProperty);
                object commandParameter = null;
                if (item.CommandParameter != null)
                {
                    BindingOperations.SetBinding(dummy, EventBinder.ValueProperty, item.CommandParameter);
                    commandParameter = dummy.GetValue(EventBinder.ValueProperty);
                }

                if (targetCommand is IEventCommand iec)
                {
                    iec.Execute(parameter[1], parameter[2], commandParameter);
                }
                else if (targetCommand is ICommand ic)
                {
                    Array.Resize(ref parameter, parameter.Length+1);
                    parameter[parameter.Length-1] = commandParameter;
                    ic.Execute(parameter);
                }
            }
        }

        private class EventBinder : FrameworkElement
        {
            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(object), typeof(EventBinder), new UIPropertyMetadata(null));
        }

    }
}
