using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace MVVM.Base.Extensions
{
    public class DebugVisible : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }
    }
}
