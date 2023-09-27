using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Localization
{
    /// <summary>
    /// Xaml Markup extension to get Localizated resource texts (to upper case)
    /// </summary>
    [ContentProperty("Text")]
    public class TranslateUpperExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;

            return LocalizationManager.GetText(Text).ToUpper();
        }
    }
}

