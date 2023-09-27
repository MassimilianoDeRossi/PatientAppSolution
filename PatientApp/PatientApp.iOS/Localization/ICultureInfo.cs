using System.Globalization;
using System.Threading;
using Foundation;
using PatientApp.iOS.Localization;
using PatientApp.Localization;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformCultureInfo))]

namespace PatientApp.iOS.Localization
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
        using (var info = new CoreTelephony.CTTelephonyNetworkInfo())
        {
          if (info.SubscriberCellularProvider != null)
          {
            CoreTelephony.CTCarrier carrier = info.SubscriberCellularProvider;
            //It returns "it" on Italy country.
            return carrier.IsoCountryCode;
          }
          return string.Empty;
        }
      }
    }

    public string DeviceOSCultureIsoCode
    {
      get
      {
        if (NSLocale.PreferredLanguages.Length > 0)
        {
          var pref = NSLocale.PreferredLanguages[0];
          return pref.Replace("_", "-");
        }
        else
          return Thread.CurrentThread.CurrentCulture.Name;

      }
    }
  }
}
