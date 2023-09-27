using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CleaningSolutionInfoPopupPage : PopupPage
    {
        public CleaningSolutionInfoPopupPage()
        {
            InitializeComponent();
        }

        // Method for animation child in PopupPage
        // Invoced after custom animation end
        //protected override Task OnAppearingAnimationEnd()
        //{
        //    //return Content.FadeTo(0.5);
        //}

        // Method for animation child in PopupPage
        // Invoked before custom animation begin
        protected override void OnDisappearingAnimationBegin()
        {
           // return Content.FadeTo(1);
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    // Prevent hide popup
        //    //return base.OnBackButtonPressed();
        //    return true;
        //}

        
    }
}