using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base
{
    public class DebugCommand : MarkupCommand
    {
        protected override void Execute(object parameter)
        {
            Debugger.Break();
        }
    }
}
