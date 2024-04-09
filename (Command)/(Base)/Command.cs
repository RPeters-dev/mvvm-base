using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM.Base
{
    public class Command : ICommand
    {
        public Action<Object> execute;

        public Predicate<Object> canExecute;

        public bool Async { get; set; }

        private event EventHandler CanExecuteChangedInternal;

        internal Command() { }

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

        public virtual bool CanExecute(Object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public virtual void Execute(Object parameter)
        {
            if (Async)
                Task.Factory.StartNew(
                    execute, parameter,
                    CancellationToken.None,
                    TaskCreationOptions.DenyChildAttach,
                    TaskScheduler.Default);
            else
                this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
                handler.Invoke(this, EventArgs.Empty);
        }

        protected static bool DefaultCanExecute(Object parameter)
        {
            return true;
        }
    }
}