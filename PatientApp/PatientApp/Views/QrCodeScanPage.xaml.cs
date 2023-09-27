using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeScanPage : BaseContentPage
    {
        bool scanFinished = false;

        public QrCodeScanPage()
        {
            InitializeComponent();
#if ENABLE_TEST_CLOUD
            btnMocked.IsVisible = true;
#else
            //btnMocked.IsVisible = System.Diagnostics.Debugger.IsAttached;
#endif            

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
        }

        protected override void OnAppearing()
        {
            if (zxing != null)
            {
                scanFinished = false;
                zxing.IsScanning = true;
            }
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            if (zxing != null)
                zxing.IsScanning = false;
            base.OnDisappearing();
        }

        private void btnLight_Clicked(object sender, EventArgs e)
        {
            if (zxing != null)
                zxing.ToggleTorch();
        }

        private void btnMocked_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<string>(App.TestModel.MockedScannedQrCode, Messaging.Messages.PRESCRIPTION_CODE_SCANNED);
        }
    }
}