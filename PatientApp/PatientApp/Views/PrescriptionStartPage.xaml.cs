using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace PatientApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PrescriptionStartPage : BaseContentPage
  {
    public PrescriptionStartPage()
    {
      InitializeComponent();
    }

    //ZXingScannerPage scanPage;
    //private void RoundedButton_Clicked(object sender, EventArgs e)
    //{
    //  if (App.TestModel != null && App.TestModel.TestModeOn)
    //  {
    //    // Test mode: mocked scan code 
    //    MessagingCenter.Send<string>(App.TestModel.MockedScannedQrCode, Messaging.Messages.PRESCRIPTION_CODE_SCANNED);
    //  }
    //  else
    //  {
    //    DoScanCode();
    //  }
    //}

    //private void DoScanCode()
    //{
    //  // Create our custom overlay
    //  var gridOverlay = new Grid()
    //  {
    //    RowSpacing = 0
    //  };
    //  gridOverlay.RowDefinitions = new RowDefinitionCollection();
    //  gridOverlay.RowDefinitions.Add(new RowDefinition()
    //  {
    //    Height = GridLength.Auto
    //  });
    //  gridOverlay.RowDefinitions.Add(new RowDefinition()
    //  {
    //    Height = GridLength.Star
    //  });
    //  gridOverlay.RowDefinitions.Add(new RowDefinition()
    //  {
    //    Height = GridLength.Auto
    //  });

    //  var layoutTop = new StackLayout
    //  {
    //    Orientation = StackOrientation.Vertical,
    //    HorizontalOptions = LayoutOptions.FillAndExpand,
    //    VerticalOptions = LayoutOptions.Start,
    //    Margin = new Thickness(20, 0, 20, 0)
    //  };
    //  var buttonLight = new Button
    //  {
    //    Text = "Light ON/OFF"
    //  };
    //  buttonLight.Clicked += delegate {
    //    scanPage.ToggleTorch();
    //  };

    //  var labelTitle = new Label()
    //  {
    //    Text = "Hold over the QR code printed on your paper prescription",
    //    Margin = new Thickness(20, 30, 20, 10),
    //    TextColor = Color.White,
    //    BackgroundColor = Color.Transparent,
    //    HorizontalOptions = LayoutOptions.Center,
    //    HorizontalTextAlignment = TextAlignment.Center
    //  };

    //  layoutTop.Children.Add(buttonLight);
    //  layoutTop.Children.Add(labelTitle);

    //  var boxViewTop = new BoxView()
    //  {
    //    BackgroundColor = Color.FromRgba(60, 60, 60, 100),
    //    HorizontalOptions = LayoutOptions.FillAndExpand,
    //    VerticalOptions = LayoutOptions.FillAndExpand
    //  };
    //  gridOverlay.Children.Add(boxViewTop);
    //  gridOverlay.Children.Add(layoutTop);
    //  Grid.SetRow(boxViewTop, 0);
    //  Grid.SetRow(layoutTop, 0);

    //  var layoutBottom = new StackLayout
    //  {
    //    Orientation = StackOrientation.Vertical,
    //    HorizontalOptions = LayoutOptions.FillAndExpand,
    //    VerticalOptions = LayoutOptions.End,
    //    Margin = new Thickness(20, 0, 20, 0)
    //  };

    //  var labelManualCode = new Label()
    //  {
    //    Text = "Do you find difficulties in scanning your access code?",
    //    Margin = new Thickness(20, 10, 20, 10),
    //    TextColor = Color.White,
    //    BackgroundColor = Color.Transparent,
    //    HorizontalOptions = LayoutOptions.Center,
    //    HorizontalTextAlignment = TextAlignment.Center
    //  };
     
    //  gridOverlay.Children.Add(layoutBottom);
    //  Grid.SetRow(layoutBottom, 2);

    //  scanPage = new ZXingScannerPage(customOverlay: gridOverlay);
    //  bool scanFinished = false;
    //  scanPage.OnScanResult += (result) =>
    //  {
    //    Device.BeginInvokeOnMainThread(async () =>
    //    {
    //      if (!scanFinished)
    //      {
    //        scanFinished = true;
    //        scanPage.IsScanning = false;
    //        //scanPage.IsAnalyzing = false;
    //        await Navigation.PopAsync();
    //        MessagingCenter.Send<string>(result.Text, Messaging.Messages.PRESCRIPTION_CODE_SCANNED);
    //      }
    //    });
    //  };

    //  TimeSpan ts = new TimeSpan(0, 0, 0, 3, 0);
    //  Device.StartTimer(ts, () => {
    //    if (scanPage.IsScanning)
    //    {
    //      scanPage.AutoFocus();
    //      return true;
    //    }
    //    else
    //    {
    //      return false;
    //    }
    //  });

    //  scanPage.IsScanning = true;
    //  //  scanPage.IsAnalyzing = true;
    //  App.NavigationController.NavigateToPage(scanPage);
    //}

  }
}
