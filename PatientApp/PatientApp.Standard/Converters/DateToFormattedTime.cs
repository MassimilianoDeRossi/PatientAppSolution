using System;
using System.Globalization;
using Xamarin.Forms;

namespace PatientApp.Converters
{
    /// <summary>
    /// Binding converter used to get formatted time from a DateTimeValue
    /// </summary>
    public class DateToFormattedTime : IValueConverter
    {
        /// <summary>
        /// Perform binding conversion
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is DateTime ? ((DateTime)value).ToString("h:mm tt") : "-";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Convert(value, targetType, parameter, culture);
    }
}
