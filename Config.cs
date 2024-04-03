using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MVVM.Base
{
    public class Config : ViewModelBase
    {
        public static Dispatcher Dispatcher { get => Instance.GetProperty<Dispatcher>(); set => Instance.SetProperty(value); }

        public static Config Instance { get; set; }
        static Config()
        {
            Instance = Get<Config>();
        }

        public Config() : base(true)
        {
        }
    }
}
