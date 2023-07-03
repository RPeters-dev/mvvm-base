using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace MVVM.Base.Models
{
    public class SelfProvidingMarkupExtension : MarkupExtension, IDisposable
    {
        static Dictionary<Type, object> instances = new Dictionary<Type, object>();
        bool? _SingleInstance;
        /// <summary>
        /// This value will be true, if not configured and the access is from a MarkupExtension
        /// </summary>
        public bool SingleInstance
        {
            get { return _SingleInstance.GetValueOrDefault(); }
            set { _SingleInstance = value; if (value && !instances.ContainsKey(GetType())) instances.Add(GetType(), this); }
        }

        public void Dispose()
        {
            if (SingleInstance == true)
                instances.Remove(GetType());
        }

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_SingleInstance == null || SingleInstance == true)
            {
                if (!instances.TryGetValue(GetType(), out var instance))
                {
                    instance = this;
                    instances[GetType()] = this;
                }

                return instance;
            }

            return this;
        }
    }
}
