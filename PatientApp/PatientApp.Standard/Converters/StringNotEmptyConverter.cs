using System;
using System.Globalization;
using Xamarin.Forms;

namespace PatientApp.Converters
{
  public class StringNotEmptyConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value != null && value.ToString().Length > 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
