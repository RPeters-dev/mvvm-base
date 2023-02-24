using System;

namespace MVVM.Base
{
    public class Command<T> : Command
    {
        public Command(Action<T> execute) : base(new Action<object>((x) => { execute.Invoke((T)(System.Convert.ChangeType( x, typeof(T)) ?? default)); }))
        {
        }

        public Command(Action<T> execute, Predicate<T> canExecute) : base(
            new Action<object>((x) => { execute.Invoke((T)(x ?? default)); }),
            new Predicate<object>((x) => { return canExecute.Invoke((T)(x ?? default)); }))
        {


        }
    }
}