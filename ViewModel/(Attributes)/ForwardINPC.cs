using System;

namespace MVVM.Base.ViewModel
{
    /// <summary>
    /// Adds a NotifyPropertyChanged forwarding to the Property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    [ViewModelInitializer]
    public class ForwardAttribute : Attribute
    {
        public string[] ForwardedProperties { get; }

        public ForwardAttribute(params string[] forwardedProperties)
        {
            ForwardedProperties=forwardedProperties;
        }

        [ViewModelInitializer]
        public static void Initialize(ViewModelBase vm)
        {
            foreach (var item in vm.GetType().GetPropertiesWithAttributes<ForwardAttribute>())
            {
                foreach (var attribute in item.Attributes)
                {
                    vm.ForwardProperyChanged(item.Property.Name, attribute.ForwardedProperties);

                }
            }
        }
    }
}
