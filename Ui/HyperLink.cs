using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace MVVM.Base.Ui
{
    public class HyperLink : UIElement
    {
        public static bool? GetNavigateProcess(DependencyObject obj)
        {
            return (bool?)obj.GetValue(NavigateProcessProperty);
        }

        public static void SetNavigateProcess(DependencyObject obj, bool? value)
        {
            obj.SetValue(NavigateProcessProperty, value);
        }

        // Using a DependencyProperty as the backing store for NavigateProcess.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigateProcessProperty =
            DependencyProperty.RegisterAttached("NavigateProcess", typeof(bool?), typeof(HyperLink), new PropertyMetadata(OnNavigateProcessChanged));

        private static void OnNavigateProcessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Hyperlink hl)
            {
                if ((bool?)e.OldValue == true)
                {
                    hl.RequestNavigate -= ExecuteHyperlink;
                }
                else if ((bool?)e.NewValue == true)
                {
                    hl.RequestNavigate += ExecuteHyperlink;
                }
            }
        }

        private static void ExecuteHyperlink(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            if (sender is Hyperlink hl)
            {
                Process.Start(e.Uri.AbsoluteUri);
                e.Handled = true;
            }
        }
    }
}
