using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MVVM.Base.ViewModel
{
    public class SelfProvidingMarkupExtension : MarkupExtension, IDisposable
    {
        public virtual void OnSingleInstaenceInitialize() {}

        static Dictionary<Type, object> instances = new Dictionary<Type, object>();
        bool? _SingleInstance;
        /// <summary>
        /// This value will be true, if not configured and the access is from a MarkupExtension
        /// </summary>
        public bool SingleInstance
        {
            get { return _SingleInstance.GetValueOrDefault(); }
            set { _SingleInstance = value; /*if (value && !instances.ContainsKey(GetType())) instances.Add(GetType(), this);*/ }
        }

        public void Dispose()
        {
            if (SingleInstance == true)
                instances.Remove(GetType());
        }

        public static T Get<T>() where T : SelfProvidingMarkupExtension
        {
            return (T)Activator.CreateInstance<T>().ProvideValue(null);
        }

        public static async Task<T> GetAsync<T>() where T : SelfProvidingMarkupExtension
        {
            return Get<T>();
        }

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_SingleInstance == null || SingleInstance == true)
            {
                if (!instances.TryGetValue(GetType(), out var instance))
                {
                    instance= instances[GetType()] = this;
                    OnSingleInstaenceInitialize();
                }

                return instance;
            }

            return this;
        }
    }
}
