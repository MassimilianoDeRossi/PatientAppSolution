using System;
using Xamarin.Forms.Xaml;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

namespace PatientApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class InfoMessagePopup : PopupPage
  {  
    public InfoMessagePopup()
    {
      
      InitializeComponent();
    }

    public InfoMessagePopup(string title, string message)
    {

      InitializeComponent();
      LblTitle.Text = title;
      LblMessage.Text = message;
    }

    protected async void btnClose_Clicked(object sender, EventArgs e)
    {
      await Navigation.PopAllPopupAsync();
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
      await Navigation.PopAllPopupAsync();
    }
  }
}