using System;

namespace MVVM.Base.ViewModel
{
    /// <summary>
    /// Used to identify Initializer Attributes and Methods
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ViewModelInitializerAttribute : Attribute
    {
        private int order = 1234;

        public int Order { get => order; set => order = value; }

    }
}
