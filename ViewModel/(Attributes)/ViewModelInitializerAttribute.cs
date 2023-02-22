using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base.ViewModel._Attributes_
{
    /// <summary>
    /// Used to identify Initializer Attributes and Methods
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ViewModelInitializerAttribute : Attribute
    {
    
    }
}
