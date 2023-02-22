using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base.Extensions
{
    public static class TypeExtensions
    {
        public static (PropertyInfo Property, T[] Attributes)[] GetPropertiesWithAttributes<T>(this Type t) where T : Attribute
        {
            return t.GetProperties().Select(p => (p, p.GetCustomAttributes<T>().ToArray() )).Where(x => x.Item2.Any()).ToArray();
        }
        public static (PropertyInfo Property, T Attribute)[] GetPropertiesWithAttribute<T>(this Type t) where T : Attribute
        {
            return t.GetProperties().Select(p => (p, p.GetCustomAttribute<T>())).Where(x => x.Item2 != null).ToArray();
        }
    }
}
