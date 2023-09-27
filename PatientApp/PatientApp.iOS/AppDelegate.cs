using System;
using System.Diagnostics;
using System.Linq;
using Foundation;
using UIKit;
using CarouselView.FormsPlugin.iOS;
using ImageCircle.Forms.Plugin.iOS;
using PCLAppConfig;
using PatientApp.iOS.Utilities;
using PatientApp.Services;
using System.Threading.Tasks;
using PatientApp.Views.Controls;
using ObjCRuntime;
using Xamarin.Forms;
using FFImageLoading.Forms.Touch;
using System.Reflection;
using PatientApp.Utilities;

namespace PatientApp.iOS
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the 
  // User Interface of the application, as well as listening (and optionally responding) to 
  // application events from iOS.
  [Register("AppDelegate")]
  public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
  {
    //
    // This method is invoked when the application has loaded and is ready to run. In this 
    // method you should instantiate the window, load the UI into it and then make the window
    // visible.
    //
    // You have 17 seconds to return from this method, or iOS will terminate your application.
    //
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      Rg.Plugins.Popup.Popup.Init();
      global::Xamarin.Forms.Forms.Init();
      global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();
      CarouselViewRenderer.Init();
      ImageCircleRenderer.Init();
      // Init FFImageLoading 
      CachedImageRenderer.Init();
      var ignore = typeof(FFImageLoading.Transformations.CropTransformation);

      Corcav.Behaviors.Infrastructure.Init();

      Assembly assembly = typeof(App).GetTypeInfo().Assembly;
      var configStream = assembly.GetManifestResourceStream("PatientApp.App.config");

      if (configStream != null)
      {
        try
        {
          ConfigurationManager.Initialise(configStream);
        }
        catch (Exception ex)
        {

        }
      }

      Octane.Xamarin.Forms.VideoPlayer.iOS.FormsVideoPlayer.Init();

      AppLoggerHelper.Init();

      AppLoggerHelper.LogEvent("TestLog", "App opened on " + DateTime.Now + " appId:" + (App.PushNotificationToken ?? "(no token)"), System.Diagnostics.TraceLevel.Info);

      AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
      {
        //HockeyAppIosHelper.LogException(e.ExceptionObject as Exception, "AppDomain unhandled exception", Newtonsoft.Json.TraceLevel.Error);
        AppLoggerHelper.LogEvent("Unhandled AppDomain Exception", e != null ? e.ToString() : "No details", System.Diagnostics.TraceLevel.Error);
      };
      TaskScheduler.UnobservedTaskException += (sender, e) =>
      {
        AppLoggerHelper.LogException(e.Exception, "TaskScheduler unhandled exception", System.Diagnostics.TraceLevel.Error);
      };

#if ENABLE_TEST_CLOUD

            Xamarin.Calabash.Start();
#endif
      /************** HockeyAPP config end ********************/
      // BITHockeyManager.SharedHockeyManager.
      // Extract build version frm manifest
      App.RuntimVersion = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();

      Settings.AppSettings.Initialize();
      DeviceLog("ApplicationInstanceId: " + Settings.AppSettings.Instance.ApplicationInstanceId);

      LoadApplication(new App(new Setup()));

      App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;
      App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;


      //MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, Share());
      // check for a notification
      if (options != null)
      {
        // check for a local notification
        if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
        {
          UILocalNotification notification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
          if (notification != null)
          {
            var itemId = notification.UserInfo["ItemId"];
            // Notify PCL layer 
            if (Services.NotificationManagerImplementation.PushListener != null)
              Services.NotificationManagerImplementation.LocalListener.OnLocalNotification(notification.ToLocalNotification());
          }
        }

        // check for a remote notification
        if (options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
        {
          NSDictionary remoteNotification = options[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
          if (remoteNotification != null)
          {
            //ManageRemoteNotification(remoteNotification);
          }
        }
      }
      else
      {
        UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
      }

      if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
      {
        var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                                     UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
        app.RegisterUserNotificationSettings(notificationSettings);
        app.RegisterForRemoteNotifications();
      }
      else
      {
        //==== register for remote notifications and get the device token
        // set what kind of notification types we want
        UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
        // register for remote notifications
        UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
      }

      // User Notification Center configuration(for IOS 10 >)
      if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
      {
        // Request notification permissions from the user
        UserNotifications.UNUserNotificationCenter.Current.RequestAuthorization(UserNotifications.UNAuthorizationOptions.Alert, (approved, err) =>
        {
            // Handle approval
          });

        // Watch for notifications while the app is active
        UserNotifications.UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
      }


      Corcav.Behaviors.Infrastructure.Init();

      UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

      //if (UIApplication.SharedApplication.CurrentUserNotificationSettings.Types == UIUserNotificationType.None)
      //{
      //    new UIAlertView("Debug", "Notifications are not allowed", null, "OK", null).Show();
      //}
      //else
      //{
      //    new UIAlertView("Debug", "Notifications are allowed", null, "OK", null).Show();
      //}

      DeviceLog("Finished Launching");

      return base.FinishedLaunching(app, options);
    }

    /// <summary>
    ///
    /// </summary>
    public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
    {
      // reset our badge
      UIApplication.SharedApplication.ApplicationIconBadgeNumber = notification.ApplicationIconBadgeNumber;

      if (Services.NotificationManagerImplementation.LocalListener != null)
      {
        Services.NotificationManagerImplementation.LocalListener.OnLocalNotification(notification.ToLocalNotification());
      }


    }

    public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
    {
      DeviceLog("ReceivedRemoteNotification");

      ManageRemoteNotification(userInfo);
    }

    public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
    {
      DeviceLog("DidReceiveRemoteNotification");

      //base.DidReceiveRemoteNotification(application, userInfo, completionHandler);
      ManageRemoteNotification(userInfo);
      completionHandler(UIBackgroundFetchResult.NewData);
    }


    /// <summary>
    /// The iOS will call the APNS in the background and issue a device token to the device. when that's
    /// accomplished, this method will be called.
    ///
    /// Note: the device token can change, so this needs to register with your server application everytime
    /// this method is invoked, or at a minimum, cache the last token and check for a change.
    /// </summary>
    public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
    {
      if (deviceToken != null)
      {
        string token = string.Empty;
        try
        {
          //token = deviceToken.ToString();                    
          //token = deviceToken.GetBase64EncodedString(NSDataBase64EncodingOptions.None);                    
          token = deviceToken.Description;
        }
        catch (Exception ex)
        {

        }
        //token = token.Replace(" ", "").TrimStart('<').TrimEnd('>');

        var bytes = deviceToken.ToArray<byte>();
        string[] hexArray = bytes.Select(b => b.ToString("x2")).ToArray();
        token = string.Join(string.Empty, hexArray);

        DeviceLog("DEVICE TOKEN: " + token);

        Services.NotificationManagerImplementation.CurrentToken = token;
        if (Services.NotificationManagerImplementation.PushListener != null)
          Services.NotificationManagerImplementation.PushListener.OnRegistered(token);
      }
    }

    /// <summary>
    /// Registering for push notifications can fail, for instance, if the device doesn't have network access.
    ///
    /// In this case, this method will be called.
    /// </summary>
    public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
    {
      new UIAlertView("Error registering push notifications", error.LocalizedDescription, default(IUIAlertViewDelegate), "OK", null).Show();
    }

    private void ManageRemoteNotification(NSDictionary userInfo)
    {
      string msg = string.Empty;

      // Extract notification properties
      var id = userInfo.ValueForKey(new NSString("id")) as NSString;
      var stype = userInfo.ValueForKey(new NSString("type")) as NSNumber;
      var aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

      if (stype != null)
      {
        if (aps != null && aps.ContainsKey(new NSString("alert")))
        {
          msg = (aps[new NSString("alert")] as NSString).ToString();
        }

        var type = (int?)stype;
        var notificationType = (RemoteNotificationType)type;

        var messageCategory = userInfo.ValueForKey(new NSString("messagecategory")) as NSNumber;

        var remoteNotification = new RemoteNotification()
        {
          Id = id,
          NotificationType = notificationType,
          Body = msg,
        };

        //Add remote notification id
        if (!string.IsNullOrEmpty(id))
          remoteNotification.Id = id;

        //Add messagecategory if is not null
        if (messageCategory != null)
        {
          remoteNotification.MessageCategory = (MotivationalMessageCategory)(int)messageCategory;
        }

        Services.NotificationManagerImplementation.PushListener.OnRemoteNotification(
                    remoteNotification);
      }
    }


    /// <summary>
    /// Handle multiple orientation only on Video View, otherwise keep only portait mode.
    /// </summary>
    /// <param name="application"></param>
    /// <param name="forWindow"></param>
    /// <returns></returns>
    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
    {
      var mainPage = Xamarin.Forms.Application.Current.MainPage;
      if (mainPage.Navigation.NavigationStack.Last() is ICustomVideoPage)
      {
        return UIInterfaceOrientationMask.AllButUpsideDown;
      }
      return UIInterfaceOrientationMask.Portrait;
    }

    private void DeviceLog(string message)
    {
      Console.WriteLine("[MyHEXPlan] {0}", message);
    }


#if ENABLE_TEST_CLOUD

        [Export("setTestScannedQrCode:")]
        public NSString setTestScannedQrCode(NSString code)
        {
            App.TestModel.MockedScannedQrCode = code;
            return NSString.Empty;
        }

        [Export("setTimeNow_SystemUtility:")]
        public NSString setTimeNow_SystemUtility(NSString date)
        {
            App.TestModel.TimeToReturn = date;
            return NSString.Empty;
        }

        [Export("setSendNotifications:")]
        public NSString setSendNotifications(NSString onBool)
        {
            bool on = false;
            Boolean.TryParse(onBool, out on);
            App.TestModel.SendNotifications = on;
            return NSString.Empty;
        }

        [Export("simulateScannedQrCode:")]
        public NSString simulateScannedQrCode(NSString dummy)
        {
            App.TestModel.ForceSkipQrScan = true;
            MessagingCenter.Send(App.TestModel.MockedScannedQrCode, Messaging.Messages.PRESCRIPTION_CODE_SCANNED);
            return NSString.Empty;
        }

        [Export("getShoppingListCheckBoxStatus:")]
        public NSString getShoppingListCheckBoxStatus(NSString dummy)
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

            return new NSString(response);
        }

        [Export("setTestMode:")]
        public NSString setTestMode(NSString onBool)
        {
            bool on = false;
            Boolean.TryParse(onBool, out on);
            App.TestModel.TestModeOn = on;
            return NSString.Empty;
        }

        [Export("getToDoList:")]
        public NSString getToDoList(NSString dummy)
        {
            return new NSString(App.TestModel.ToDoList);
        }

        [Export("getDoneList:")]
        public NSString getDoneList(NSString dummy)
        {
            return new NSString(App.TestModel.DoneList);
        }

        [Export("getSelectedStrutDirectionImage:")]
        public NSString getSelectedStrutDirectionImage(NSString dummy)
        {
            return new NSString(App.TestModel.SelectedStrutDirectionImage);
        }

        [Export("getSelectedStrutBackgroundImageName:")]
        public NSString getSelectedStrutBackgroundImageName(NSString dummy)
        {
            return new NSString(App.TestModel.SelectedStrutBackgroundImageName);
        }

        [Export("getBadgeStrutsAdj:")]
        public string GetBadgeStrutsAdj(NSString dummy)
        {
            return App.TestModel.BadgeStrutsAdj;
        }

        [Export("getFrameStrutsAdj:")]
        public string GetFrameStrutsAdj(NSString dummy)
        {
            return App.TestModel.FrameStrutsAdj;
        }
#endif
  }
}
