using System;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVM.Base
{

    public abstract class RoutedCommand : ContextMarkupCommand
    {
        /// <summary>
        /// its protected to prevent this from showing up in xaml
        /// </summary>
        protected ICommand CommandTarget { get; set; }

        /// <summary>
        /// Used to Bind the Targetcomand use a <see cref="ICommand"/> or <see cref="Binding"/>
        /// </summary>
        public object Command { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            base.ProvideValue(serviceProvider);

            if (CommandTarget != null)
                return this;

            //the command is filled direct if a markup extension is used
            if (Command is ICommand ic)
                CommandTarget = ic;

            if (TargetInstance != null && Command is Binding cb)
            {
                var dummy = new BindingDummy(TargetInstance.DataContext);

                CommandTarget = dummy.GetValue(cb) as ICommand;

                if (CommandTarget is RoutedCommand rc)
                {
                    rc.TargetInstance = TargetInstance;
                    rc.ProvideValue(serviceProvider);
                }
            }

            return this;
        }

        public override bool CanExecute(object parameter) => CommandTarget != null && CommandTarget.CanExecute(parameter);
    }
}
