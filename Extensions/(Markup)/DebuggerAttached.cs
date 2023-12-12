using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MVVM.Base.Extensions
{
    [MarkupExtensionReturnType(typeof(Boolean))]
    public class DebuggerAttached : MarkupExtension
    {
        private bool? isDebuggerAttached;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return isDebuggerAttached = isDebuggerAttached ?? System.Diagnostics.Debugger.IsAttached;
        }
    }
}
