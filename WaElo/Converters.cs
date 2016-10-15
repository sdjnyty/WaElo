using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WaElo
{
  public static class Converters
  {
    private static StringToIntConverter stringToIntConverter;

    public static StringToIntConverter StringToIntConverter { get { return stringToIntConverter; } }

    static Converters()
    {
      stringToIntConverter = new StringToIntConverter();
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

}
