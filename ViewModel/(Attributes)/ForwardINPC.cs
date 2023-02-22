using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM.Base.Extensions;

namespace MVVM.Base.ViewModel._Attributes_
{
    /// <summary>
    /// Adds a NotifyPropertyChanged forwarding to the Property
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    [ViewModelInitializer]
    public class ForwardINPCAttribute : Attribute
    {

        [ViewModelInitializer]
        public static void Initialize(ViewModelBase vm)
        {
            foreach (var item in vm.GetType().GetPropertiesWithAttributes<ForwardINPCAttribute>())
            {
                vm.ForwardProperyChanged(item.Property.Name);
            }
        }
    }
}
