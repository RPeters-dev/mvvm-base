using System.Collections.ObjectModel;
using System.Windows;

namespace MVVM.Base
{
    /// <summary>
    /// Provides the base class for a collection of <see cref="EventBinding"/>.
    /// </summary>
    public class EventBinders : Collection<EventBinding>
    {
        internal void AddBindings(DependencyObject d)
        {
            foreach (var item in this)
            {
                item.Attach(d);
            }
        }

        internal void RemoveBindings(DependencyObject d)
        {
            foreach (var item in this)
            {
                item.Detach(d);
            }
        }
    }
}
