using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace HackUTDBikeQueryApp.WPF.Converters
{
    public class Base64ToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (!(value is string)) throw new ArgumentException("Only string values are supported.");

            // Convert base 64 string to byte[]
            byte[] imageBytes = System.Convert.FromBase64String(value as string);

            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes)) //new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                BitmapImage image = new BitmapImage();

                ms.Position = 0;

                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = ms;
                image.EndInit();

                image.Freeze();

                return image;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
