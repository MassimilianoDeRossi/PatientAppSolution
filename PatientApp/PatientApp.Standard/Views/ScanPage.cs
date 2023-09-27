using PatientApp.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace PatientApp.Views
{
    public class ScanPage : BaseContentPage
    {
        ZXingScannerView zxing = null;
        //ZXingDefaultOverlay overlay = null;

        public ScanPage()
        {
            bool scanFinished = false;

            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingScannerView",
            };

            zxing.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!scanFinished)
                    {
                        scanFinished = true;
                        zxing.IsScanning = false;
                        MessagingCenter.Send<string>(result.Text, Messaging.Messages.PRESCRIPTION_CODE_SCANNED);
                    }
                });
            };

            TimeSpan ts = new TimeSpan(0, 0, 0, 3, 0);
            Device.StartTimer(ts, () =>
            {
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

            var overlay = GetOverlayContent();

            var header = new HeaderControl()
            {
                 Title = "Scan prescription code"
            };

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            grid.RowDefinitions.Add(new RowDefinition() { Height = 40 });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.Children.Add(header);
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);
            Grid.SetRow(header, 0);
            Grid.SetRow(zxing, 1);
            Grid.SetRow(overlay, 1);
            Content = grid;
        }

        protected override void OnAppearing()
        {
            if (zxing != null)
                zxing.IsScanning = true;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            if (zxing != null)
                zxing.IsScanning = false;
            base.OnDisappearing();
        }

        private Grid GetOverlayContent()
        {
            // Create our custom overlay
            var gridOverlay = new Grid()
            {
                RowSpacing = 0
            };
            gridOverlay.RowDefinitions = new RowDefinitionCollection();
            gridOverlay.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });
            gridOverlay.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Star
            });
            gridOverlay.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });

            var layoutTop = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 0, 20, 0)
            };
            var buttonLight = new Button
            {
                Text = "Light ON/OFF"
            };
            buttonLight.Clicked += delegate {
                if (zxing != null)
                    zxing.ToggleTorch();
            };

            var labelTitle = new Label()
            {
                Text = "Hold over the QR code printed on your paper prescription",
                Margin = new Thickness(20, 30, 20, 10),
                TextColor = Color.White,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            layoutTop.Children.Add(buttonLight);
            layoutTop.Children.Add(labelTitle);

            var boxViewTop = new BoxView()
            {
                BackgroundColor = Color.FromRgba(60, 60, 60, 100),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            gridOverlay.Children.Add(boxViewTop);
            gridOverlay.Children.Add(layoutTop);
            Grid.SetRow(boxViewTop, 0);
            Grid.SetRow(layoutTop, 0);

            var layoutBottom = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(20, 0, 20, 0)
            };

            gridOverlay.Children.Add(layoutBottom);
            Grid.SetRow(layoutBottom, 2);

            return gridOverlay;
        }
    }
}
