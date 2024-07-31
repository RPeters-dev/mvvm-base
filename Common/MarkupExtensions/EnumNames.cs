using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MVVM.Base.Common.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(string[]))]
    public class EnumNames : MarkupExtension
    {
        public EnumNames(Type sourceType)
        {
            SourceType = sourceType;
        }
        [ConstructorArgument("sourceType")]
        public Type SourceType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetNames(SourceType);
        }
    }
}
