using System;
using System.Globalization;
using Xamarin.Forms;

namespace PatientApp.Converters
{
    /// <summary>
    /// Binding converted used to invert the boolean logic
    /// </summary>
    public class InverterConverter : IValueConverter
    {
        /// <summary>
        /// Perform binding conversion 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>True if value is false - False if value is True</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !((bool)value);
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
