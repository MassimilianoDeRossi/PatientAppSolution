using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Localization
{
    public class Translator : INotifyPropertyChanged
    {
        public string this[string text]
        {
            get
            {
                return LocalizationManager.GetText(text);
            }
        }

        public static Translator Instance { get; } = new Translator();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }

    //public class ImageTranslator : INotifyPropertyChanged
    //{
    //    public string this[string text]
    //    {
    //        get
    //        {
    //            return LocalizationManager.GetImage(text);
    //        }
    //    }

    //    public static ImageTranslator Instance { get; } = new ImageTranslator();

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public void Invalidate()
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    //    }
    //}

    /// <summary>
    /// Xaml Markup extension to get Localizated resource texts
    /// </summary>
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public TranslateExtension()
        {

        }

        public TranslateExtension(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;

            //return LocalizationManager.GetText(Text);

            var binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Text}]",
                Source = Translator.Instance,
            };
            return binding;
        }
    }
}

