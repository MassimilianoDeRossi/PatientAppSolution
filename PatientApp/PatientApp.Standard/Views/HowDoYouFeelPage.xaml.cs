using Xamarin.Forms.Xaml;

namespace PatientApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HowDoYouFeelPage : BaseContentPage
    {
        public HowDoYouFeelPage()
        {
            InitializeComponent();
        }

        //private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        //{
        //    //var action = DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Confirm this mood");
        //    var selectMoodPopup = new SelectMoodPopupPage();

        //    await Navigation.PushPopupAsync(selectMoodPopup);

        //    //await PopupNavigation.PushAsync(selectMoodPopup);
        //}

    }
}