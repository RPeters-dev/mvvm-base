using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace MVVM.Base.Models
{
    public class SelfProvidingMarkupExtension : MarkupExtension, IDisposable
    {
        Dictionary<Type, object> instances = new Dictionary<Type, object>();
        public bool SingleInstance { get; set; }

        public void Dispose()
        {
            if(SingleInstance)
                instances.Remove(GetType());
        }

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if(SingleInstance)
            {
                if(!instances.TryGetValue(GetType(), out var instance))
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
