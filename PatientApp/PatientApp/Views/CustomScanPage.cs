using System;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace PatientApp.Views
{
  public class CustomScanPage : ContentPage
  {
    private ZXingScannerView zxing;
    private ZXingDefaultOverlay overlay;
    private Action<string> _onCodeScanned;


    public CustomScanPage(Action<string> onCodeScanned) : base()
    {
      _onCodeScanned = onCodeScanned;

      zxing = new ZXingScannerView
      {
        HorizontalOptions = LayoutOptions.FillAndExpand,
        VerticalOptions = LayoutOptions.FillAndExpand,
        AutomationId = "zxingScannerView",
      };
      zxing.OnScanResult += (result) =>
          Device.BeginInvokeOnMainThread(async () =>
          {
            // Stop analysis until we navigate away so we don't keep reading barcodes
            zxing.IsAnalyzing = false;

            // Navigate away
            await Navigation.PopAsync();

            if (_onCodeScanned != null)
              onCodeScanned(result.Text);
          });
      
      overlay = new ZXingDefaultOverlay
      {
        ShowFlashButton = zxing.HasTorch,
        AutomationId = "zxingDefaultOverlay",
      };
      overlay.FlashButtonClicked += (sender, e) =>
      {
        zxing.IsTorchOn = !zxing.IsTorchOn;
      };
      var grid = new Grid
      {
        VerticalOptions = LayoutOptions.FillAndExpand,
        HorizontalOptions = LayoutOptions.FillAndExpand,
      };
      grid.Children.Add(zxing);
      grid.Children.Add(overlay);

      // The root page of your application
      Content = grid;

      TimeSpan ts = new TimeSpan(0, 0, 0, 3, 0);
      Device.StartTimer(ts, () => {
        if (zxing.IsScanning)
        {
          zxing.AutoFocus();
          return true;
        }
        else
        {
          return false;
        }
      });
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();

      zxing.IsScanning = true;
    }

    protected override void OnDisappearing()
    {
      zxing.IsScanning = false;

      base.OnDisappearing();
    }
  }
}
