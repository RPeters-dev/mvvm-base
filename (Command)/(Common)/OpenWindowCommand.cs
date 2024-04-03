using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.Base
{
    public class OpenWindowCommand : ContextMarkupCommand
    {
        public bool Dialog { get; set; } = false;
        public bool KeepContext { get; set; } = true;
        public double? Width { get; set; }
        public double? Height { get; set; }

        protected override void Execute(object parameter)
        {
            if(parameter == null) { return; }
            var window = parameter as Window;
            if (parameter is Type T)
            {
                if (T.IsAssignableFrom(typeof(Window)))
                {
                    window = Activator.CreateInstance(T) as Window;
                }
                else if (T.IsAssignableFrom(typeof(FrameworkElement)))
                {
                    window = new Window();
                    window.Content = Activator.CreateInstance(T) as FrameworkElement;
                }
            }
            else if(parameter is DataTemplate dt) 
            {
                window = new Window();
                window.Content = dt.LoadContent();
            }
            if (window == null) { return; }

            if (KeepContext)
                window.DataContext = DataContext;

            if(Width != null)
            window.Width = Width.Value;
            if (Height != null)
            window.Height = Height.Value;

            if (Dialog)
                window.ShowDialog();
            else
                window.Show();
        }
    }
}
