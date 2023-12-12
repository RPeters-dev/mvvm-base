using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MVVM.Base
{
    [MarkupExtensionReturnType(typeof(DataTemplateSelector))]
    public class _TypeTemplateSelector : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new TypeTemplateSelector();
        }
    }

    public class TypeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item == null)
            {
                return null;
            }

            IEnumerable<ResourceDictionary> searchCollection()
            {
                if(container is FrameworkElement fwe)
                yield return fwe.Resources;
                yield return Application.Current.Resources;
            };

            var itemType = item?.GetType() ?? typeof(object);

            foreach (var res in searchCollection())
            {
                var result = res[new DataTemplateKey(itemType)];
                if (result is DataTemplate dt)
                    return dt;
            }        

            return null;
        }
    }
}
