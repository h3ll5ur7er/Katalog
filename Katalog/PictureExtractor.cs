using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Katalog
{
    public class PictureExtractor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var path = value.ToString().Split(';')[0];
                if (!File.Exists(path)) path = @"C:\temp\remoteSnapshots\Camera1\9b9e78a2-f85a-46f3-a46b-0b5d7b7061c9.bmp";
                var url = new Uri(path, UriKind.RelativeOrAbsolute);
                return url;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}