using System;
using System.Globalization;
using Xamarin.Forms;

using PatientApp.Localization;

namespace PatientApp.Converters
{
    /// <summary>
    /// Binding converter used to get formatted time from a DateTimeValue
    /// </summary>
    public class LocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return LocalizationManager.GetText(value.ToString());
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
