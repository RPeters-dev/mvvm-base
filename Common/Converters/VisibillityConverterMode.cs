using System;

namespace MVVM.Base.Common.Converters
{
    [Flags]
    public enum VisibilityConverterMode
    {
        Hide = 1,
        Collapsing = 2,
        Inverted =3,
    }
}
