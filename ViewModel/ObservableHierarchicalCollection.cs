using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace MVVM.Base.ViewModel
{
    public class ObservableHierarchicalCollection<T> : ObservableCollection<T>, IObservableHierarchicalCollection where T : INotifyPropertyChanged
    {
        public ObservableHierarchicalCollection(Func<T, IEnumerable<T>> childSelector)
        {
            ChildSelector  = childSelector;

            CollectionChanged += ObservableHieracCollection_CollectionChanged;
        }

        public virtual event PropertyChangedEventHandler ChildPropertyChanged
        { add { _childPropertyChanged += value; } remove { _childPropertyChanged -= value; } }

        private event PropertyChangedEventHandler _childPropertyChanged;

        public IEnumerable<object> AllChildren => SelectRecursive(this);
        public IEnumerable<T> AllChildrenOfT => SelectRecursive(this).OfType<T>();

        public Func<T, IEnumerable<T>> ChildSelector { get; }

        Func<object, IEnumerable> IObservableHierarchicalCollection.ChildSelector => (x) => ChildSelector.Invoke((T)x);

        internal IEnumerable<object> SelectRecursive(object itemsSource)
        {
            if (itemsSource is IObservableHierarchicalCollection source)
                foreach (var item in source)
                {
                    if (!(item is T))
                        continue;

                    yield return item;

                    foreach (var _item in SelectRecursive(source.ChildSelector.Invoke(item)))
                    {
                        yield return _item;
                    }
                }
        }

        private void ObservableHieracCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (T item in e.NewItems)
                {
                    item.PropertyChanged += PassNPC;
                    if (ChildSelector.Invoke((T)item) is IObservableHierarchicalCollection ioc)
                        ioc.ChildPropertyChanged += PassNPC;
                }

            if (e.OldItems != null)
                foreach (T item in e.OldItems)
                {
                    item.PropertyChanged -= PassNPC;
                    if (ChildSelector.Invoke((T)item) is IObservableHierarchicalCollection ioc)
                        ioc.ChildPropertyChanged -= PassNPC;
                }
        }

        private void PassNPC(object sender, PropertyChangedEventArgs e)
        {
            _childPropertyChanged?.Invoke(this, e);
        }
    }
}
