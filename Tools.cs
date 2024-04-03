using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MVVM.Base
{
    public class Tools
    {
        public static void DispatcherExecute(Action callback, DispatcherPriority priority = DispatcherPriority.DataBind)
        {
            if (!Config.Dispatcher.CheckAccess())
            {
                Config.Dispatcher.Invoke(() =>
                {
                    callback.Invoke();
                }, priority);
            }
            else
            {
                callback.Invoke();
            }
        }
    }
}
