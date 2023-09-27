using System;
using System.Globalization;
using Xamarin.Forms;

namespace PatientApp.Converters
{
    /// <summary>
    /// Binding converted for string value checking.
    /// Convert the string value to true if it is equal to the binding parameter
    /// </summary>
    public class StringValueEqualsConverter : IValueConverter
    {
        /// <summary>
        /// Perform bonding conversion
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>True if value is equal to parameter - false if not</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                return value.ToString().Equals(parameter.ToString(), StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                return value.ToString().Equals(parameter.ToString(), StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }
    }
}
