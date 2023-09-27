using System;
using System.Globalization;
using Xamarin.Forms;

namespace PatientApp.Converters
{
    /// <summary>
    /// Binding converter for Boolean to float mapping
    /// The true and false values can be customized
    /// </summary>
    public class BoolToFloatConverter : IValueConverter
    {
        /// <summary>
        /// Value mapped to true 
        /// </summary>
        public float TrueValue { get; set; } = 1;

        /// <summary>
        /// Valued mapped to false
        /// </summary>
        public float FalseValue { get; set; } = 0;

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
            if (value != null && value is bool)
            {
                return ((bool)value) ? TrueValue : FalseValue;
            }
            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
