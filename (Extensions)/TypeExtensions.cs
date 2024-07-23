using System;
using System.Linq;
using System.Reflection;

namespace MVVM.Base
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
