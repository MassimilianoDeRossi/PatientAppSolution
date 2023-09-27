using Xamarin.Forms.Xaml;

namespace PatientApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupportPage : BaseContentPage
    {
        public SupportPage()
        {
            InitializeComponent();
            this.Appearing += SupportPage_Appearing;
        }

        private void SupportPage_Appearing(object sender, System.EventArgs e)
        {
            aiBusy.IsVisible = true;
        }

        private void WebView_Navigating(object sender, Xamarin.Forms.WebNavigatingEventArgs e)
        {
            aiBusy.IsVisible = true;
        }

        private void WebView_Navigated(object sender, Xamarin.Forms.WebNavigatedEventArgs e)
        {
            aiBusy.IsVisible = false;
        }

    }
}