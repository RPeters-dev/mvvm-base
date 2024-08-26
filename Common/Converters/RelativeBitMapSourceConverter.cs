using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MVVM.Base.Common.Converters
{
    public class RelativeBitMapSourceConverter : MarkupConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var imagePath = value.ToString() ;
            var imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.UriSource = new Uri(imagePath, UriKind.Relative);
            imageSource.EndInit();
            //it needs some access
            _ = imageSource.Height;
            return imageSource;
        }
    }
}
