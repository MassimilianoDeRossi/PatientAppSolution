using System;
using System.Globalization;
using Xamarin.Forms;

namespace PatientApp.Converters
{
    /// <summary>
    /// Binding Converter for Boolean to Color mapping
    /// The true and false values can be customized
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        /// <summary>
        /// Color mapped to true value 
        /// </summary>
        public Color TrueColor { get; set; }

        /// <summary>
        /// Color mapped to false value
        /// </summary>
        public Color FalseColor { get; set; }

        /// <summary>
        /// Performs binding conersion
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                return ((bool)value) ? TrueColor : FalseColor;
            }
            return FalseColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
