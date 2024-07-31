using System.Windows;

namespace MVVM.Base.Common.Converters
{
    public class ConverterValuesVisibility : BooleanConverter_ValueProvider
    {
        private VisibilityConverterMode _Mode;

        public bool Inverted
        {
            set
            {
                if (value)
                    Mode |= VisibilityConverterMode.Inverted;
                else 
                    Mode &= ~VisibilityConverterMode.Inverted;
            }
        }
        public VisibilityConverterMode Mode
        {
            set
            {
                _Mode = value;
                if (value.HasFlag(VisibilityConverterMode.Collapsing))
                {
                    FalseValue = Visibility.Collapsed;
                }else if (value.HasFlag(VisibilityConverterMode.Hide))
                {
                    FalseValue = Visibility.Hidden;
                }

                if (value.HasFlag(VisibilityConverterMode.Inverted))
                {
                    var ot = TrueValue;
                    TrueValue = FalseValue;
                    FalseValue = ot;
                }
            }
            get { return _Mode; }
        }

        public ConverterValuesVisibility()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
            NullValue = Visibility.Hidden;
        }
    }

    public class VisibilityConverter : BooleanConverter
    {
        public VisibilityConverter()
        {
            ValueProvider = new ConverterValuesVisibility();
        }

        public VisibilityConverterMode Mode { set => ValueProvider.Mode = value; }
        public bool Inverted { set => ValueProvider.Inverted = value; }

        public new ConverterValuesVisibility ValueProvider { get => (ConverterValuesVisibility)base.ValueProvider; set => base.ValueProvider = value; }
    }
}
