using System;
using System.Globalization;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;

namespace WaElo
{
  public static class Converters
  {

    public static StringToIntConverter StringToIntConverter = new StringToIntConverter();

    public static WAgameToBitmapImageConverter WAgameToBitmapImageConverter = new WAgameToBitmapImageConverter();

    static Converters()
    {

    }
  }

  public class StringToIntConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (int)double.Parse(value as string);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class WAgameToBitmapImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return null;
      var wagame = value as WAgame;
      using (var br = new BinaryReader(new FileStream(wagame.FullFileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
      {
        br.ReadInt32();
        var mapLength = br.ReadInt32() - 8;
        if (br.ReadInt32() == 3)
        {
          using (var stream = new MemoryStream(br.ReadBytes(mapLength)))
          {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.StreamSource = stream;
            bi.EndInit();
            return bi;
          }
        }
        else
        {
          return Application.Current.FindResource("noMap");
        }
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

}
