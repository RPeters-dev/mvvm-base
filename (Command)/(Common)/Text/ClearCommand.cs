using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base.CommonCommands.Text
{
    public class ClearCommand : MarkupCommand
    {
        protected override void Execute(object parameter)
        {
            switch (parameter)
            {
                case System.Windows.Controls.TextBox textBox:
                    textBox.Clear();
                    break;
                default:
                    break;
            }
        }
    }
}
