using System.ComponentModel;

namespace MVVM.Base
{
    public class RefeshViewItem : MarkupCommand<ICollectionView>
    {
        protected override void Execute(ICollectionView parameter) => parameter?.Refresh();
    }
}