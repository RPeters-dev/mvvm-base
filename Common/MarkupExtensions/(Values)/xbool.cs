using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MVVM.Base.Common.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(bool))]
    public class xbool : MarkupExtension
    {
        public xbool(bool value)
        {
            Value = value;
        }

        public bool Value { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}
