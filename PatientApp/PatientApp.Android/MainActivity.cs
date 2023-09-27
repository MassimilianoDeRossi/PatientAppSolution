using System;
using System.Threading.Tasks;
using System.Reflection;

using Xamarin.Forms;
using Android.App;
using Android.Content.PM;
using Android.OS;

using FFImageLoading.Forms.Droid;
using Plugin.Permissions;
using PCLAppConfig;
using ImageCircle.Forms.Plugin.Droid;
using CarouselView.FormsPlugin.Droid;


using PatientApp.Utilities;
using PatientApp.Services;
using PatientApp.Droid.Notifications;
using PatientApp.Droid.Services;
using Android.Support.V7.App;
using System.Diagnostics;

#if ENABLE_TEST_CLOUD
using Java.Interop;
#endif

namespace PatientApp.Droid
{
  [Activity(Label = "myHEXplan",
            Icon = "@drawable/appicon",
            Theme = "@style/MainTheme",
            //MainLauncher = true,
            ScreenOrientation = ScreenOrientation.Portrait,
            ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
  {
    public static MainActivity Instance = null;

    protected override void OnCreate(Bundle bundle)
    {
      AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
      Instance = this;
      base.OnCreate(bundle);
      SQLitePCL.Batteries_V2.Init();

      if (App.RuntimVersion == null)
      {
        Assembly assembly = typeof(App).GetTypeInfo().Assembly;
        var configStream = assembly.GetManifestResourceStream("PatientApp.App.config");

        if (configStream != null)
        {
          try
          {
            ConfigurationManager.Initialise(configStream);
          }
          catch
          {
          }
        }
      }

      AppLoggerHelper.Init();

      AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
      {

        AppLoggerHelper.LogException(e.ExceptionObject as Exception, "Unhandled AppDomain Exception", TraceLevel.Error);
      };
      TaskScheduler.UnobservedTaskException += (sender, e) =>
      {
        AppLoggerHelper.LogException(e.Exception, "TaskScheduler unhandled exception", TraceLevel.Error);
      };

      Xamarin.Essentials.Platform.Init(this, bundle);
      Rg.Plugins.Popup.Popup.Init(this, bundle);
      global::Xamarin.Forms.Forms.Init(this, bundle);
      CarouselViewRenderer.Init();
      global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
      ImageCircleRenderer.Init();

      // Init FFImageLoading 
      CachedImageRenderer.Init(true);
      var ignore = typeof(FFImageLoading.Transformations.CropTransformation);

      // Extract build version from manifest            
      App.RuntimVersion = this.ApplicationContext.PackageManager.GetPackageInfo(this.ApplicationContext.PackageName, 0).VersionName;
      Settings.AppSettings.Initialize();

      LoadApplication(new App(new Setup()));

      RestorePopup();
      //FormsVideoPlayer.Init("C816CFDDC2375A5423761131B6544537126C9241");
      Octane.Xamarin.Forms.VideoPlayer.Android.FormsVideoPlayer.Init();

      var density = Resources.DisplayMetrics.DensityDpi;

      Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
      App.ScreenWidth = ConvertPixelsToDp(Resources.DisplayMetrics.WidthPixels);
      App.ScreenHeight = (int)ConvertPixelsToDp(Resources.DisplayMetrics.HeightPixels); // real pixels
    }

    public async override void OnBackPressed()
    {
      if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
      {
        // Do something if there are some pages in the `PopupStack`
        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
      }
      else
      {
        // Do something if there are not any pages in the `PopupStack`
      }
    }

    private void RestorePopup()
    {
      //If tapped on remote notification
      //var activity = (MainActivity)Forms.Context;
      var activity = MainActivity.Instance;
      if (activity != null && activity.Intent != null)
      {
        var intent = activity.Intent;
        if (intent.Extras != null && intent.HasExtra(RemoteNotificationIntent.ID)) //If tap on remote notification
        {
          var remoteNotification = new RemoteNotification()
          {
            Id = intent.GetStringExtra(RemoteNotificationIntent.ID),
            MessageCategory = (MotivationalMessageCategory)intent.GetIntExtra(RemoteNotificationIntent.MESSAGE_CATEGORY, 0),
            NotificationType = (RemoteNotificationType)intent.GetIntExtra(RemoteNotificationIntent.TYPE, 0),
            Body = intent.GetStringExtra(RemoteNotificationIntent.MESSAGE)
          };
          Services.NotificationManagerImplementation.PushListener.OnRemoteNotification(remoteNotification);
        }

        if (intent.Extras != null && intent.HasExtra(LocalNotificationIntent.ID)) //If tap on local notification
        {
          var localNotification = new LocalNotification()
          {
            Title = intent.GetStringExtra(LocalNotificationIntent.TITLE),
            NotificationType = (LocalNotificationType)intent.GetIntExtra(LocalNotificationIntent.TYPE, 0),
            Body = intent.GetStringExtra(LocalNotificationIntent.MESSAGE)
          };
          Services.NotificationManagerImplementation.LocalListener.OnLocalNotification(localNotification);
        }
      }
    }

    private int ConvertPixelsToDp(float pixelValue)
    {
      var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
      return dp;
    }

    protected override void OnResume()
    {
      base.OnResume();
      PatientApp.Utilities.AppLoggerHelper.LogEvent("TestLog", "App opened on " + DateTime.Now + " appId:" + (App.PushNotificationToken ?? "(no token)"), TraceLevel.Info);
    }


    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
      PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

#if ENABLE_TEST_CLOUD
    [Export("TestExport")]
    public void TestExport()
    {
    }

    [Export("SetTestMode")]
    public void SetTestMode(string onBool)
    {
      bool on = false;
      Boolean.TryParse(onBool, out on);
      App.TestModel.TestModeOn = on;
    }

    [Export("SetTestScannedQrCode")]
    public void SetTestScannedQrCode(string code)
    {
      App.TestModel.MockedScannedQrCode = code;
    }

    [Export("SetTimeNow_SystemUtility")]
    public void SetTimeNow_SystemUtility(string date)
    {
      App.TestModel.TimeToReturn = date;
    }

    [Export("SetSendNotifications")]
    public void SetSendNotifications(string onBool)
    {
      bool on = false;
      Boolean.TryParse(onBool, out on);
      App.TestModel.SendNotifications = on;
    }

    [Export("SimulateScannedQrCode")]
    public void SimulateScannedQrCode(string dummy)
    {
      App.TestModel.ForceSkipQrScan = true;
      MessagingCenter.Send(App.TestModel.MockedScannedQrCode, Messaging.Messages.PRESCRIPTION_CODE_SCANNED);
    }

    [Export("GetShoppingListCheckBoxStatus")]
    public string GetShoppingListCheckBoxStatus(string dummy)
    {
      var arrayToConvert = App.TestModel.ShoppingListCheckBoxStatus;
      string response = "";

      for (int i = 0; i < arrayToConvert.Length; i++)
      {
        response += arrayToConvert[i].ToString() + ",";
      }

      if (response.Length > 0)
      {
        response = response.Remove(response.Length - 1); //remove the ',' as last char
      }

      return response;
    }

    [Export("GetToDoList")]
    public string GetToDoList(string dummy)
    {
      return App.TestModel.ToDoList;
    }

    [Export("GetDoneList")]
    public string GetDoneList(string dummy)
    {
      return App.TestModel.DoneList;
    }

    [Export("GetSelectedStrutDirectionImage")]
    public string GetSelectedStrutDirectionImage(string dummy)
    {
      return App.TestModel.SelectedStrutDirectionImage;
    }

    [Export("GetSelectedStrutBackgroundImageName")]
    public string GetSelectedStrutBackgroundImageName(string dummy)
    {
      return App.TestModel.SelectedStrutBackgroundImageName;
    }

    [Export("GetAndroidVersion")]
    public int GetAndroidVersion(string dummy)
    {
      return (int)Build.VERSION.SdkInt;
    }

    [Export("GetBadgeStrutsAdj")]
    public string GetBadgeStrutsAdj(string dummy)
    {
      return App.TestModel.BadgeStrutsAdj;
    }

    [Export("GetFrameStrutsAdj")]
    public string GetFrameStrutsAdj(string dummy)
    {
      return App.TestModel.FrameStrutsAdj;
    }
#endif
  }
}
