using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base
{
    public class ModuloConverter : MarkupConverter
    {
        public ModuloConverter(int modulo)
        {
                Modulo = modulo;
        }

        public int Modulo { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Int32 int32val)
                return int32val % Modulo;
            if (value is UInt32 uint32val)
                return uint32val % Modulo;
            if (value is Int16 int16val)
                return int16val % Modulo;
            if (value is UInt16 uint16val)
                return uint16val % Modulo;
            if (value is Int64 int64val)
                return int64val % Modulo;
            if (value is UInt64 uint64val)
                return uint64val % (UInt64)Modulo;

            if (Int32.TryParse(value.ToString(), out int result))
                return result % Modulo;

            return value;
        }
    }
}
