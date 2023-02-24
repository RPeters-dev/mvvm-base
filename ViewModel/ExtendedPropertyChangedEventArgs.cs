using System.Collections.Generic;
using System.ComponentModel;

namespace MVVM.Base.ViewModel
{
    /// <summary>
    /// <inheritdoc cref="PropertyChangedEventArgs"/>
    /// </summary>
    public class ExtendedPropertyChangedEventArgs : PropertyChangedEventArgs
    {

        public Stack<string> Route = new Stack<string>();

        /// <summary>
        /// initializes a new instance of <see cref="ExtendedPropertyChangedEventArgs"/>
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <param name="oldValue">The old value of the property that changed.</param>
        /// <param name="newValue">The new value of the property that changed.</param>
        public ExtendedPropertyChangedEventArgs(string propertyName, object oldValue, object newValue) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
        /// <summary>
        /// The old value of the property that changed
        /// </summary>
        public object OldValue { get; }
        /// <summary>
        /// The new value of the property that changed
        /// </summary>
        public object NewValue { get; }
    }
}
