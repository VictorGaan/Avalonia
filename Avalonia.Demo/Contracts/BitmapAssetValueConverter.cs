using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Avalonia.Demo.Contracts;
public class BitmapAssetValueConverter : IValueConverter
{
    public static BitmapAssetValueConverter Instance = new BitmapAssetValueConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        if (value is string rawUri && targetType.IsAssignableFrom(typeof(Bitmap)))
        {
            Uri uri;
            string assemblyName = Assembly.GetEntryAssembly().GetName().Name;
            if (rawUri.StartsWith("avares://"))
            {
                uri = new Uri(rawUri);
            }
            else
            {
                uri = new Uri($"avares://{assemblyName}/{rawUri}");
            }

            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            Stream asset;
            try
            {
                asset = assets.Open(uri);
            }
            catch (Exception e)
            {
                uri = new Uri($"avares://{assemblyName}/Images/default.png");
                asset = assets.Open(uri);
            }

            return new Bitmap(asset);
        }

        throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}