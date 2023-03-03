using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVM.Base
{
    public partial class EventBinding
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
                Debugger.Log(0, "Error", $"{typeof(BindingDummy)} Error: can't find Event '{EventName}' on '{d.GetType()}'");
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
                Debugger.Log(0, "Error", $"{typeof(BindingDummy)} argument error: first argument of '{eventName}' must be a FrameworkElement");
                return;
            }

            var eventBinders = EventBindings.GetEventBinders(sender)?.Where(x => x.EventName.Equals(eventName));
            if (eventBinders == null)
            {
                eventBinders = new[] { EventBindings.GetEventObject(sender) };
            }

            var dummy = new BindingDummy(sender.DataContext);

            foreach (var item in eventBinders)
            {
                BindingOperations.SetBinding(dummy, BindingDummy.ValueProperty, item.Command);
                var targetCommand = dummy.GetValue(BindingDummy.ValueProperty);
                object commandParameter = null;
                if (item.CommandParameter != null)
                {
                    BindingOperations.SetBinding(dummy, BindingDummy.ValueProperty, item.CommandParameter);
                    commandParameter = dummy.GetValue(BindingDummy.ValueProperty);
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

    }
}
