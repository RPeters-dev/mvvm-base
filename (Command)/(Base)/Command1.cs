using System;
using System.Threading.Tasks;

namespace MVVM.Base
{
    public class Command<T> : Command
    {
        public bool ValidateParameter { get; set; } = true;
        public Command(Action<T> execute, bool validateType = true) : base(
            new Action<object>((x) => { execute.Invoke((T)(TrychangeType(x) ?? default)); }))
        {
            if (validateType)
                canExecute = defaultCanExecute;
        }

        private static bool defaultCanExecute(object p)
        {
            if (p == null) return false;

            return typeof(T).IsAssignableFrom(p.GetType());
        }

        public Command(Func<T, Task> execute) : base(new Action<object>((x) => { Task.Run(() => execute((T)TrychangeType(x))); }))
        {
            canExecute = defaultCanExecute;
        }

        private static object TrychangeType(object obj)
        {
            if(obj == null)
                return default(T);

            if (typeof(T).IsAssignableFrom(obj.GetType())) return obj;
            try
            {
                return Convert.ChangeType(obj, typeof(T));
            }
            catch { return null; }

        }

        public Command(Action<T> execute, Predicate<T> canExecute) : base(
            new Action<object>((x) => { execute.Invoke((T)(x ?? default)); }),
            new Predicate<object>((x) => { return canExecute.Invoke((T)(x ?? default)); }))
        {

        }
    }
}