using System;
using System.Globalization;
using Xamarin.Forms;

namespace PatientApp.Converters
{
    /// <summary>
    /// Convert enum value to string.
    /// </summary>
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Enum ? $"{value}" : "undefined";
        } 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Convert(value, targetType, parameter, culture);
    }
}
