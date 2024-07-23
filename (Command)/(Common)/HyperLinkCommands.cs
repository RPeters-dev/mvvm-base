using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MVVM.Base
{
    public enum HyperLinkModes
    {
        Process,
    }

    public class ExecuteHyperLinkCommand : ContextMarkupCommand
    {
        public HyperLinkModes Mode { get; set; }

        protected override void Execute(object parameter)
        {
            if(TargetInstance is Hyperlink hl)
            {
                switch (Mode)
                {
                    case HyperLinkModes.Process:
                        Process.Start(hl.NavigateUri.AbsoluteUri);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
