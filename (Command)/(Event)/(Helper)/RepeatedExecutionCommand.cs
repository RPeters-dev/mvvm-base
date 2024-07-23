using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVM.Base
{
    public class RepeatedExecutionCommand : RoutedCommand
    {


        public int DletaMs { set => Delta = TimeSpan.FromMilliseconds(value); }

        public TimeSpan Delta { get; set; } = TimeSpan.MaxValue;
        public int Repeats { get; set; } = 2;


        DateTime _time = DateTime.MaxValue;
        int index = 0;

        public RepeatedExecutionCommand(Binding commandTarget) : base(commandTarget)
        {
        }

        public RepeatedExecutionCommand()
        {
        }

        protected override void Execute(object parameter)
        {
            var reset = true;
            //do the double test
            if (DateTime.Now - _time < Delta)
            {
                reset = false;
                if (++index == Repeats)
                    CommandTarget.Execute(parameter);
            }
            if(reset)
            {
                _time = DateTime.Now;
                index = 0;
            }
        }
    }
}
