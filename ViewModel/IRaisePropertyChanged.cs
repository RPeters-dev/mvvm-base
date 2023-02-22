using System.ComponentModel;

namespace MVVM.Base
{
    /// <summary>
    /// Provides a Method to Raise the <see cref="INotifyPropertyChanged"/> event
    /// </summary>
    public interface IRaisePropertyChanged
    {
        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged"/>
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        void RaisePropertyChanged(string propertyName);
    }

}