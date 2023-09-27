using System;
using Xamarin.Forms.Xaml;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

namespace PatientApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class TimeLapseImageZoomPopup : PopupPage
  {
    public TimeLapseImageZoomPopup()
    {
      
      InitializeComponent();
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
      await Navigation.PopAllPopupAsync();
    }
  }
}