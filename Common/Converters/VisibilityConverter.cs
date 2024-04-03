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
            FalseValue = Visibility.Hidden;
            NullValue = Visibility.Collapsed;
        }
    }

    public class VisibilityConverter : BooleanConverter
    {
        public VisibilityConverter()
        {
            ValueProvider = new BooleanConverter_ValueProvider();
        }

        public new BooleanConverter_ValueProvider ValueProvider { get => base.ValueProvider; set => base.ValueProvider = value; }
    }
}
