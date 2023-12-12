using System;
using System.Windows.Input;
using System.Windows.Markup;

namespace MVVM.Base
{
    public abstract class MarkupCommand : MarkupExtension, ICommand
    {
        #region Events

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; this.CanExecuteChangedInternal += value; }
            remove { CommandManager.RequerySuggested -= value; this.CanExecuteChangedInternal -= value; }
        }

        private event EventHandler CanExecuteChangedInternal;

        #endregion Events

        #region Methods

        public virtual bool CanExecute(Object parameter)
        {
            return true; ;
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
                handler.Invoke(this, EventArgs.Empty);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        void ICommand.Execute(object parameter)
        {
            this.Execute(parameter);
        }

        protected abstract void Execute(object parameter);

        private static bool DefaultCanExecute(Object parameter)
        {
            return true;
        }

        #endregion Methods
    }
}