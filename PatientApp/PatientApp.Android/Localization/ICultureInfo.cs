using System.Globalization;
using System.Linq;
using System.Threading;
using Android.Content;
using Android.Telephony;
using PatientApp.Droid.Localization;
using PatientApp.Localization;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformCultureInfo))]

namespace PatientApp.Droid.Localization
{
    public class PlatformCultureInfo : ICultureInfo
    {
        public System.Globalization.CultureInfo CurrentCulture
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture;
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = value;
            }
        }

        public System.Globalization.CultureInfo CurrentUICulture
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                Thread.CurrentThread.CurrentUICulture = value;
            }
        }

        public string SIMCountryIso
        {
            get
            {
                TelephonyManager telephonyManager = (TelephonyManager)Android.App.Application.Context.GetSystemService(Context.TelephonyService);
                //Get the ISO of the network country (fe: 'it' on Italy). 
                //// This property works also if the mobile is connect only on wifi network without a SIM but there is no documentation that support the situation.
                //var networkCountryIso = telephonyManager?.NetworkCountryIso;
                //If in the mobile there is a SIM, this property return the SIM country (fe.: it on Italian mobile company)
                return telephonyManager?.SimCountryIso;
            }
        }

        public string DeviceOSCultureIsoCode => Thread.CurrentThread.CurrentCulture.Name;
  }
}