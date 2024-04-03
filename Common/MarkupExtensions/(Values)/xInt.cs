using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MVVM.Base.Common.MarkupExtensions
{

    [MarkupExtensionReturnType(typeof(int))]
    public class xint : MarkupExtension
    {
        public xint(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}
