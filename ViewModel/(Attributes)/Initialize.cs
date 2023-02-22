using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM.Base.Extensions;

namespace MVVM.Base.ViewModel._Attributes_
{

    /// <summary>
    /// Initializes the Property with its default Constructor
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    [ViewModelInitializer]

    public class InitializeAttribute : Attribute
    {
        public Type Type { get; set; }

        [ViewModelInitializer]
        public static void Initialize(ViewModelBase vm)
        {
            foreach (var item in vm.GetType().GetPropertiesWithAttribute<InitializeAttribute>())
            {
                vm.SetProperty(Activator.CreateInstance(item.Attribute.Type ?? item.Property.PropertyType), item.Property.Name);
            }
        }
    }
}
