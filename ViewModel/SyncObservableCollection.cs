using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MVVM.Base.ViewModel
{
    public class SyncObservableCollection<T> : ObservableCollection<T>
    {
        public IEnumerable<T> Enummerate()
        {
            return view.Cast<T>();
        }

        private readonly ICollectionView view;

        public ICollectionView View
        {
            get { return view; }
        }

        public SyncObservableCollection()
        {
            if (!Config.Dispatcher.CheckAccess())
                throw new Exception("Collection have to be initialized from the Dispatcher Thread");

            view = CollectionViewSource.GetDefaultView(this);

            view.CollectionChanged += (s, e) => { if (e.Action == NotifyCollectionChangedAction.Reset) Refreshed?.Invoke(s,e); };
        }

        public event EventHandler Refreshed;

        protected override void ClearItems()
        {
            Tools.DispatcherExecute(() => base.ClearItems());           
        }

        protected override void InsertItem(int index, T item)
        {
            Tools.DispatcherExecute(() => base.InsertItem(index, item));
        }

        protected override void RemoveItem(int index)
        {
            Tools.DispatcherExecute(() => base.RemoveItem(index));
        }

        public void SetFilter(Predicate<T> filter)
        {
            view.Filter = (x) => filter.Invoke((T)x);
        }
    }
}
