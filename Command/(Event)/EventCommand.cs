using System.Windows.Input;

namespace MVVM.Base
{

    public interface IEventCommand : ICommand
    {
        void Execute(object sender, object e, object parameter);
    }
    public class EventCommand<THandler> : Command, IEventCommand
    {


        public delegate void CommandHandler(object sender, THandler e, object parameter);

        public EventCommand(CommandHandler execute) : base((x) => execute.Invoke((x as object[])[0], (THandler)(x as object[])[1], (x as object[])[2]))
        {
        }

        public void Execute(object sender, object e, object parameter)
        {
            base.Execute(new object[] { sender, e,parameter });
        }
    }
}
