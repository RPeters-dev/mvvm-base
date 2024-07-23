using MVVM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MVVM.Base
{
    public class TranslationManager
    {
        internal static Dictionary<string, TranslationValue> Values { get; set; } = new Dictionary<string, TranslationValue>();

        public static void Set(string key, string value)
        {
            if (Values.ContainsKey(key)) { Values[key].Value = value; }
            else
                Values.Add(key, new TranslationValue() { Value = value });
        }

        public static string Get(string key)
        {
            return internalGet(key);
        }

        internal static TranslationValue internalGet(string key)
        {
            if (Values.TryGetValue(key, out var value)) return value;

            Set(key, key + "?");
            return Values[key];
        }
    }
    
    internal class TranslationValue : ViewModelBase
    {
        public string Value { get => GetProperty<string>(); set => SetProperty(value); }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator string(TranslationValue source)
        { return source.Value; }
    }
    
    public class Translation : MarkupExtension
    {
        [ConstructorArgument("key")]
        public string Key { get; set; }
        public Translation(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new Binding(nameof(TranslationValue.Value)) { Source = TranslationManager.internalGet(Key) }.ProvideValue(serviceProvider);
        }
    }
}
