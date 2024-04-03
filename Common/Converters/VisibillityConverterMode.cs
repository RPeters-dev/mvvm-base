using System;

namespace MVVM.Base.Common.Converters
{
    [Flags]
    public enum VisibilityConverterMode
    {
        Normal = 0,
        Collapsing = 1,
        Inverted = 2,
    }
}
