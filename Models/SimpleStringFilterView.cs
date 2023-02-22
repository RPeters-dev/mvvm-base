using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVM.Base
{
    public class SimpleStringFilterView : CollectionViewSource, INotifyPropertyChanged
    {
        public Dictionary<string, object> AttachedProperties = new Dictionary<string, object>();

        private string _filterString;

        public SimpleStringFilterView()
        {
            Filter += FilterDelegate;
            Command_Refresh = new Command(Refresh);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand Command_Refresh { get; set; }

        public string FilterString
        {
            get { return _filterString; }
            set { _filterString = value; RaisePropertyChanged(); }
        }

        public Action<object, FilterEventArgs, string> StringFilter { get; set; }

        public object this[string index]
        {
            get
            {
                AttachedProperties.TryGetValue(index, out object result);
                return result;
            }
            set { AttachedProperties[index] = value; }
        }

        public void FilterDelegate(object sender, FilterEventArgs e) => StringFilter?.Invoke(sender, e, FilterString);

        public void RaisePropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            View.Refresh();
        }

        public void Refresh() => View.Refresh();
    }
}
