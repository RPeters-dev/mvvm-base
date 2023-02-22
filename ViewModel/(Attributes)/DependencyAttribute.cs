using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MVVM.Base.Extensions;

namespace MVVM.Base.ViewModel._Attributes_
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    [ViewModelInitializer]
    public class DependencyAttribute : Attribute
    {
        public string[] dependencies;

        public DependencyAttribute(params string[] _dependencies)
        {
            dependencies = _dependencies ?? new string[0];
        }

        public string[] Dependencies
        {
            get { return dependencies; }
        }

        [ViewModelInitializer]
        public static void Initialize(ViewModelBase vm)
        {
            foreach (var item in vm.GetType().GetPropertiesWithAttributes<DependencyAttribute>())
            {
                vm.AddSupplier(item.Property.Name, item.Attributes.SelectMany(x => x.Dependencies).ToArray());
            }
        }
    }
}
