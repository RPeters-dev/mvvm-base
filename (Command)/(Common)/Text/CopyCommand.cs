using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MVVM.Base.CommonCommands.Text
{
    public class CopyCommand : MarkupCommand
    {
        public enum Modes
        {
            SelectedText,
            AllText
        }

        public Modes Mode { get; set; } = Modes.SelectedText;

        protected override void Execute(object parameter)
        {
            string text = String.Empty;

            if (Mode == Modes.AllText)
            {
                switch (parameter)
                {
                    case TextBox textBox:
                        text = textBox.Text;
                        break;
                    default:
                        return;
                }
            }
            else
                switch (parameter)
                {
                    case TextBox textBox:
                        text = textBox.SelectedText;
                        break;
                    default:
                        return;
                }

            Clipboard.SetText(text);

        }
    }
}
