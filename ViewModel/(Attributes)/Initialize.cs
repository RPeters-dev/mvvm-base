using System;
namespace MVVM.Base.ViewModel
{

    /// <summary>
    /// Initializes the Property with its default Constructor
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    [ViewModelInitializer]

    public class InitializeAttribute : Attribute
    {
        public InitializeAttribute(bool dispatcherInitialize = false)
        {
            DispatcherInitialize = dispatcherInitialize;
        }

        public Type Type { get; set; }
        public bool DispatcherInitialize { get; }

        [ViewModelInitializer(Order = 1)]
        public static void Initialize(ViewModelBase vm)
        {
            foreach (var item in vm.GetType().GetPropertiesWithAttribute<InitializeAttribute>())
            {
                if (item.Attribute.DispatcherInitialize)
                {
                    Config.Dispatcher.Invoke(() => vm.SetProperty(Activator.CreateInstance(item.Attribute.Type ?? item.Property.PropertyType), item.Property.Name)
                    ,
                     System.Windows.Threading.DispatcherPriority.DataBind);
                }
                else
                    vm.SetProperty(Activator.CreateInstance(item.Attribute.Type ?? item.Property.PropertyType), item.Property.Name);
            }
        }
    }
}
