using Android.App;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace PatientApp.Droid.Utilities
{
    public class AppLoggerAndroidHelper : PatientApp.Utilities.AppLoggerFormHelper
    {
        private static readonly string _appId;

        static AppLoggerAndroidHelper()
        {
            _appId = PCLAppConfig.ConfigurationManager.AppSettings["AppLoggerId"];
        }

        public static void Init()
        {
            AppCenter.Start(_appId, typeof(Analytics), typeof(Crashes));
        }
    }
}