using System;
using System.Collections;
using System.ComponentModel;

namespace MVVM.Base
{
    public interface IObservableHierarchicalCollection : IEnumerable
    {
        Func<object, IEnumerable> ChildSelector { get; }

        event PropertyChangedEventHandler ChildPropertyChanged;
    }
}
