using PatientApp.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinSiteCareVideoPage : BaseContentPage, ICustomVideoPage
    {

        public PinSiteCareVideoPage()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Catch video ortientation changes
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            // Video plugin has an issue on Android platform: video is stretched 
            if (Device.RuntimePlatform == Device.Android)
            {
                // workaround
                if (width > height)
                {
                    // Landscape
                    videoPlayer.HeightRequest = 300;
                }
                else
                {
                    // Portrait
                    videoPlayer.HeightRequest = 200;
                }
            }

            base.OnSizeAllocated(width, height);
        }
    }
}