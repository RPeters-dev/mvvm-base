using System;
using System.Windows.Input;

namespace MVVM.Base
{
    public class Command : ICommand
    {
        public Action<Object> execute;

        protected Predicate<Object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public Command(Action execute) : this(new Action<object>(delegate (object x) { execute.Invoke(); }))
        {
        }

        public Command(Action execute, Predicate<Object> canExecute) : this(new Action<object>(delegate (object x) { execute.Invoke(); }), canExecute)
        {
        }

        public Command(Action<Object> execute) : this(execute, DefaultCanExecute)
        {
        }

        public Command(Action<Object> execute, Predicate<Object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; this.CanExecuteChangedInternal += value; }
            remove { CommandManager.RequerySuggested -= value; this.CanExecuteChangedInternal -= value; }
        }

        public bool CanExecute(Object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(Object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
                handler.Invoke(this, EventArgs.Empty);
        }

        private static bool DefaultCanExecute(Object parameter)
        {
            return true;
        }
    }
}