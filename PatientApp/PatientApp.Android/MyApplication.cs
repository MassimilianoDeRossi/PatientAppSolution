using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Common;

namespace PatientApp.Droid
{
  [Application]
  public class MyApplication : Application
  {
    public static Context AppContext;

    public MyApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {

    }

    public override void OnCreate()
    {
      base.OnCreate();

      AppContext = this.ApplicationContext;

      if (IsPlayServicesAvailable())
      {
        // Start the registration intent service; try to get a token:
        var intent = new Intent(AppContext, typeof(RegistrationIntentService));
        StartService(intent);
        //This service will keep your app receiving push even when closed.             
        StartPushService();
      }
    }

    public static void StartPushService()
    {
      AppContext.StartService(new Intent(AppContext, typeof(PushNotificationService)));

      if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
      {

        PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(PushNotificationService)), 0);
        AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
        alarm.Cancel(pintent);
      }
    }

    public static void StopPushService()
    {
      AppContext.StopService(new Intent(AppContext, typeof(PushNotificationService)));
      if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
      {
        PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(PushNotificationService)), 0);
        AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
        alarm.Cancel(pintent);
      }
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