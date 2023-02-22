using System.Windows;

namespace MVVM.Base
{
    public class VisibilityConverter : BooleanConverter
    {
        public VisibilityConverterMode Mode
        {
            set
            {
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
        }

        public VisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Hidden;
            NullValue = Visibility.Collapsed;
        }
    }
}
