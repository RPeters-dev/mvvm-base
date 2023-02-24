using System;
using MVVM.Base.Extensions;

namespace MVVM.Base.ViewModel
{

    /// <summary>
    /// Initializes the Property with its default Constructor
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    [ViewModelInitializer]

    public class InitializeAttribute : Attribute
    {
        public Type Type { get; set; }

        [ViewModelInitializer(Order = 1)]
        public static void Initialize(ViewModelBase vm)
        {
            foreach (var item in vm.GetType().GetPropertiesWithAttribute<InitializeAttribute>())
            {
                vm.SetProperty(Activator.CreateInstance(item.Attribute.Type ?? item.Property.PropertyType), item.Property.Name);
            }
        }
    }
}
