using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace PatientApp.UITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            IApp app;

            if (platform == Platform.Android)
            {
                var appConf = ConfigureApp
                        .Android
                        .PreferIdeSettings()
                        .InstalledApp("com.orthofix.myhexplan.on");

                if (!Utils.ScreenshotEnabled)
                {
                    appConf.DisableSpecFlowIntegration();
                }

                app = appConf.StartApp();
            }
            else
            {
#if DEBUG
                app = ConfigureApp.iOS.EnableLocalScreenshots().DeviceIdentifier("13FAF86B-8011-48F0-BD21-315E2292E642").AppBundle("../../../PatientApp/PatientApp.iOS/bin/iPhoneSimulator/Debug/device-builds/iphone10.4-11.2/PatientApp.iOS.app").StartApp();
                //return ConfigureApp.iOS.InstalledApp("com.orthofix.myhexplan").EnableLocalScreenshots().StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear);
#else
                var appConf = ConfigureApp
                    .iOS
                    .InstalledApp("com.orthofix.myhexplan.on");

                if (!Utils.ScreenshotEnabled)
                {
                    appConf.DisableSpecFlowIntegration();
                }

                app = appConf.StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear);
#endif
            }
            Utils.Init(platform, app);
            CustomAssert.Init(platform, app);
            return app;
        }
    }
}

//device identifier iphone 8 iOS 11.2: "13FAF86B-8011-48F0-BD21-315E2292E642"
//return ConfigureApp.iOS.EnableLocalScreenshots().DeviceIdentifier("13FAF86B-8011-48F0-BD21-315E2292E642").AppBundle("../../../PatientApp/PatientApp.iOS/bin/iPhoneSimulator/Debug/device-builds/iphone10.4-11.2/PatientApp.iOS.app").StartApp();

//device identifier iphont 6 iOS 11.2: "EF34FD29-35D6-4219-928C-5FBE61F876A6"
//return ConfigureApp.iOS.EnableLocalScreenshots().DeviceIdentifier("EF34FD29-35D6-4219-928C-5FBE61F876A6").AppBundle("../../../PatientApp/PatientApp.iOS/bin/iPhoneSimulator/Debug/device-builds/iphone7.2-11.2/PatientApp.iOS.app").StartApp();

//device identifier iphont 7 iOS 11.2: "8374A8E7-CE04-4611-8C24-C93973EE749D"
//return ConfigureApp.iOS.EnableLocalScreenshots().DeviceIdentifier("8374A8E7-CE04-4611-8C24-C93973EE749D").AppBundle("../../../PatientApp/PatientApp.iOS/bin/iPhoneSimulator/Debug/device-builds/iphone9.1-11.2/PatientApp.iOS.app").StartApp();

//device identifier iphont SE iOS 11.2: "17839BED-8554-4B6D-9330-735D14361CEB"
//return ConfigureApp.iOS.EnableLocalScreenshots().DeviceIdentifier("17839BED-8554-4B6D-9330-735D14361CEB").AppBundle("../../../PatientApp/PatientApp.iOS/bin/iPhoneSimulator/Debug/device-builds/iphone8.4-11.2/PatientApp.iOS.app").StartApp();