using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using Android.Gms.Common;

namespace PatientApp.Droid
{
    [Activity(Theme = "@style/Theme.Splash", //Indicates the theme to use for this activity
               Label = "myHEXplan",
               Icon = "@drawable/appicon",
               MainLauncher = true, //Set it as boot activity
               NoHistory = true)] //Doesn't place it in back stack
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        
#if !ENABLE_TEST_CLOUD
            System.Threading.Thread.Sleep(1500); //Let's wait awhile...
#endif
            this.StartActivity(typeof(MainActivity));
        }  

        public bool IsPlayServicesAvailable()
        {
            string msg;
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    msg = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    msg = "Sorry, this device is not supported";
                    //Finish();
                }
                return false;
            }
            else
            {
                msg = "Google Play Services is available.";
                return true;
            }
        }

    }
}