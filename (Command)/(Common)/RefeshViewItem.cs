using System.ComponentModel;

namespace MVVM.Base
{
    public class RefeshViewItem : MarkupCommand<ICollectionView>
    {
        public override void Execute(ICollectionView parameter) => parameter?.Refresh();
    }
}