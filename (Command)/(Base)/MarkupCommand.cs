namespace MVVM.Base
{
    public abstract class MarkupCommand<T> : MarkupCommand
    {
        #region Methods

        public virtual bool CanExecute(T parameter)
        {
            return true;
        }

        public override bool CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        public abstract void Execute(T parameter);

        protected override void Execute(object parameter)
        {
            Execute((T)parameter);
        }

        #endregion Methods
    }
}