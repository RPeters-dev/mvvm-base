using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace MVVM.Base
{
    public class FakeBinding : MarkupExtension
    {
        public FakeBinding(string path)
        {
            Path = path;
        }

        public string Path { get; }
        public IValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public string ElementName { get; set; }
        public RelativeSource RelativeSource { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var result = new Binding();
            if (Path != null) result.Path = new PropertyPath(Path, (object[])null);
            if (Converter != null) result.Converter = Converter;
            if (ConverterParameter != null) result.ConverterParameter = ConverterParameter;
            if (ElementName != null) result.ElementName = ElementName;
            if (RelativeSource != null) result.RelativeSource = RelativeSource;
            return result;
        }
    }


    public abstract class RoutedCommand : ContextMarkupCommand
    {
        /// <summary>
        /// its protected to prevent this from showing up in xaml
        /// </summary>
        protected ICommand CommandTarget { get; set; }

        public RoutedCommand(Binding commandTarget)
        {
            Command = commandTarget;
        }
        public RoutedCommand()
        {
            CommandTarget = null;
        }

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

            if (TargetInstance is FrameworkElement fe && Command is Binding cb)
            {
                var dummy = new BindingDummy(fe);

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
