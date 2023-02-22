using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using MVVM.Base.Models;
using MVVM.Base.ViewModel;
using MVVM.Base.ViewModel._Attributes_;

namespace MVVM.Base
{
    /// <summary>
    /// BAseclass for all ViewModels
    /// </summary>
    public class ViewModelBase : SelfProvidingMarkupExtension, INotifyPropertyChanged, IRaisePropertyChanged
    {
        #region INotifyPropertyChanged member
        public virtual event PropertyChangedEventHandler PropertyChanged
        { add { _propertyChanged += value; } remove { _propertyChanged -= value; } }

        private event PropertyChangedEventHandler _propertyChanged;
        #endregion

        /// <summary>
        /// Contains information ablut all Initializers in this Project
        /// </summary>
        private static MethodInfo[] Initializers { get; }

        /// <summary>
        /// Contains PropertyChanged Dependencies, values will be called when the key value has changed
        /// </summary>
        public Dictionary<string, List<string>> Dependencies { get; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// Contains all values of the VM
        /// </summary>
        public Dictionary<string, object> Values { get; private set; } = new Dictionary<string, object>();


        public Dictionary<string, List<Action<ViewModelBase, PropertyChangedEventArgs>>> PropertyChangedActions { get; private set; }
            = new Dictionary<string, List<Action<ViewModelBase, PropertyChangedEventArgs>>>();

        /// <summary>
        /// static Constructor
        /// </summary>
        static ViewModelBase()
        {
            Initializers = Assembly.GetAssembly(typeof(ViewModelBase)).GetTypes().Select(t => new { t, attributres = t.GetCustomAttributes<ViewModelInitializerAttribute>() })
                .Where(t => t.attributres.Any()).SelectMany(t => t.t.GetMethods().Where(m => m.GetCustomAttributes<ViewModelInitializerAttribute>().Any())).ToArray();
        }


        /// <summary>
        /// Initializes a new Insatnce of <see cref="ViewModelBase"/>
        /// </summary>
        public ViewModelBase()
        {
            RunInitializer();
            Initialized();
        }

        /// <summary>
        /// Executes all Initializers
        /// </summary>
        private void RunInitializer()
        {
            foreach (var item in Initializers)
            {
                item.Invoke(item.DeclaringType, new[] { this });
            }
        }

        /// <summary>
        /// Adds a new PropertyChanged Dependency
        /// </summary>
        /// <param name="source">the property the <paramref name="dependencies"/> depend on</param>
        /// <param name="dependencies">the dependencies of <paramref name="source"/></param>
        public void AddDependency(string source, params string[] dependencies)
        {
            var exists = Dependencies.TryGetValue(source, out var result);
            if (!exists)
                Dependencies.Add(source, result = new List<string>());

            result.AddRange(dependencies.Except(result).ToArray());
        }

        /// <summary>
        /// Adds an action to the PropertyChangedEvent of <paramref name="property"/>
        /// </summary>
        /// <param name="property">a change of this property will execute <paramref name="action"/></param>
        /// <param name="action">the action that will be executed</param>
        public void AddPropertyChangedAction(string property, Action<ViewModelBase, PropertyChangedEventArgs> action)
        {
            var exists = PropertyChangedActions.TryGetValue(property, out var result);
            if (!exists)
                PropertyChangedActions.Add(property, result = new List<Action<ViewModelBase, PropertyChangedEventArgs>>());

            result.Add(action);
        }

        /// <summary>
        /// Adds a Dependency for <paramref name="source"/> on every supplier
        /// </summary>
        /// <param name="source">the dependent property</param>
        /// <param name="suppliers">the supplying properties</param>
        public void AddSupplier(string source, params string[] suppliers)
        {
            foreach (var item in suppliers)
            {
                AddDependency(item, source);
            }
        }

        /// <summary>
        /// Forwards a <see cref="INotifyPropertyChanged"/> event to the current instance
        /// </summary>
        /// <param name="source">object whose <see cref="INotifyPropertyChanged"/> events will be forwarded</param>
        public void ForwardProperyChanged(INotifyPropertyChanged source)
        {
            if (source != null)
                source.PropertyChanged += ForwardPropertyChanged;
        }

        /// <summary>
        /// Forwards a <see cref="INotifyPropertyChanged"/> event to the current instance
        /// </summary>
        /// <param name="source">property whose <see cref="INotifyPropertyChanged"/> events will be forwarded</param>
        public void ForwardProperyChanged(string name)
        {
            UpdateForwardProperyChanged(this, new ExtendedPropertyChangedEventArgs(name, null, GetProperty<object>(name)));

            AddPropertyChangedAction(name, UpdateForwardProperyChanged);
        }

        /// <summary>
        /// Updates a ProperyChanged forwarding
        /// </summary>
        private void UpdateForwardProperyChanged(ViewModelBase obj, PropertyChangedEventArgs e)
        {
            if (e is ExtendedPropertyChangedEventArgs epc)
            {
                if (epc.OldValue is INotifyPropertyChanged ov)
                    ov.PropertyChanged -= obj.ForwardPropertyChanged;
                if (epc.NewValue is INotifyPropertyChanged nv)
                    nv.PropertyChanged += obj.ForwardPropertyChanged;
            }
        }

        /// <inheritdoc/>
        public virtual new object MemberwiseClone()
        {
            var result = (ViewModelBase)base.MemberwiseClone();
            result.Values = new Dictionary<string, object>(Values);
            return result;
        }

        /// <summary>
        /// Raise the <see cref="INotifyPropertyChanged"/> event
        /// </summary>
        /// <param name="p"></param>
        public void RaisePropertyChanged([CallerMemberName] string p = "") => RaisePropertyChanged(p);

        /// Raise the <see cref="INotifyPropertyChanged"/> event with <see cref="ExtendedPropertyChangedEventArgs"/>
        public void RaisePropertyChanged([CallerMemberName] string _propertyName = "", object oldValue = null, object newValue = null)
        {
            if (_propertyChanged != null)
            {
                var pce = new ExtendedPropertyChangedEventArgs(_propertyName, oldValue, newValue);
                _propertyChanged(this, pce);

                if (Dependencies.TryGetValue(_propertyName, out var result))
                {
                    foreach (var item in result)
                    {
                        RaisePropertyChanged(item);
                    }
                }
                if (PropertyChangedActions.TryGetValue(_propertyName, out var dp))
                {
                    foreach (var item in dp)
                    {
                        item.Invoke(this, pce);
                    }
                }
            }
        }

        /// <summary>
        /// Routes <see cref="INotifyPropertyChanged"/> to the <paramref name="raisePropertyChangedHandler"/>
        /// </summary>
        /// <param name="raisePropertyChangedHandler">the handler for the <see cref="INotifyPropertyChanged"/> events</param>
        public void RoutePropertyChanged(Action<string> raisePropertyChangedHandler)
        {
            _propertyChanged += (x, y) => raisePropertyChangedHandler.Invoke(y.PropertyName);
        }

        /// <summary>
        /// Forward a  <see cref="INotifyPropertyChanged"/> event
        /// </summary>
        private void ForwardPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(sender, e);
            }
        }

        /// <summary>
        /// Gets the value of the property with the given name
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="propertyName">name of the Property</param>
        /// <returns>value of the property  <paramref name="propertyName"/></returns>
        public T GetProperty<T>(string propertyName) => GetProperty<T>(false, propertyName);

        /// <summary>
        /// Gets the value of the property with the given name
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="initialize">Initializes the value with the defualt constructor if not done</param>
        /// <param name="propertyName">name of the Property</param>
        /// <returns>value of the property  <paramref name="propertyName"/></returns>
        public T GetProperty<T>(bool initialize = false, [CallerMemberName] string propertyName = "")
        {
            if (!Values.ContainsKey(propertyName))
            {
                if (!initialize)
                    return default;
                else
                    SetProperty(Activator.CreateInstance<T>(), propertyName);
            }
            return (T)Values[propertyName];
        }

        /// <summary>
        /// Sets the value of a Property
        /// </summary>
        /// <param name="value">the new Value</param>
        /// <param name="propertyName">the name of the property</param>
        public void SetProperty(object value, [CallerMemberName] string propertyName = "")
        {
            Values.TryGetValue(propertyName, out var oldValue);

            if (oldValue == value)
                return;

            Values[propertyName] = value;
            RaisePropertyChanged(propertyName, oldValue, value);
        }
        /// <summary>
        /// Sets the value of a Property
        /// </summary>
        /// <param name="value">the new Value</param>
        /// <param name="target">does not use the <see cref="Values"/> use another target instead</param>
        /// <param name="propertyName">the name of the property</param>
        public void SetProperty(object value, ref object target, [CallerMemberName] string propertyName = "")
        {
            var oldValue = target;
            target = value;
            RaisePropertyChanged(propertyName, oldValue, value);
        }


        public virtual void Initialized()
        { } 
    }
}