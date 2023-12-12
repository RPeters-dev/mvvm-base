using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base.Models
{
    public class FlatObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        private void ObservableHieracCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (T item in e.NewItems)
                {
                    item.PropertyChanged += PassNPC;
                  
                }

            if (e.OldItems != null)
                foreach (T item in e.OldItems)
                {
                    item.PropertyChanged -= PassNPC;
                }
        }

        private void PassNPC(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Item." + e.PropertyName ));
        }
    }
}
