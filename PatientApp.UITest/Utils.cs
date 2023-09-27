using PatientApp.UITest.Resources;
using System;
using System.Globalization;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace PatientApp.UITest
{
    public static class Utils
    {
        private static Platform _platform;
        private static IApp _app;
        private static string _language;
        private static CultureInfo _cultureInfo;
        private static bool? _screenshotEnabled;

        /// <summary>
        /// Default timeout when query
        /// </summary>
        public const int DefaultQueryTimeoutSeconds = 20;

        /// <summary>
        /// Get the culture used for the test
        /// </summary>
        public static CultureInfo CultureInfo { get { return _cultureInfo; } }

        /// <summary>
        /// Indicates if the test is extecuting on Android
        /// </summary>
        public static bool isAndroid { get { return _platform == Platform.Android; } }

        /// <summary>
        /// Indicates if the test is extecuting on IOS
        /// </summary>
        public static bool isIos { get { return _platform == Platform.iOS; } }

        /// <summary>
        /// Indicates if during test execution on app center it will take screenshots (manually and automatic with specflow)
        /// </summary>
        public static bool ScreenshotEnabled {
            get
            {
                if (_screenshotEnabled == null)
                {
                    Console.WriteLine("DEBUG_TEST_INFO isAndroid = " + isAndroid);
                    Console.WriteLine("DEBUG_TEST_INFO isiOs = " + isIos);
                    string resName = isAndroid ? "enableScreenshot_Android" : "enableScreenshot_iOS";

                    var screenshotEnabledTemp = false;
                    bool.TryParse(Config.ResourceManager.GetString(resName), out screenshotEnabledTemp);
                    _screenshotEnabled = screenshotEnabledTemp;

                    Console.WriteLine("DEBUG_TEST_INFO screenshotEnabled = " + _screenshotEnabled);
                }
                return (bool)_screenshotEnabled;
            }
        }

        /// <summary>
        /// Initialize utility data. Used only when test starts
        /// </summary>
        public static void Init(Platform platform, IApp app)
        {
            _platform = platform;
            _app = app;
            _cultureInfo = new CultureInfo("en-US");
            _language = CultureInfo.TwoLetterISOLanguageName;
        }

        /// <summary>
        /// Get backdoor name based on running platform
        /// </summary>
        public static string GetBackdoorName(Backdoors resName)
        {
            if (isAndroid)
            {
                return BackdoorAndroid.ResourceManager.GetString(resName.ToString());
            }
            if (isIos)
            {
                return BackdoorIOS.ResourceManager.GetString(resName.ToString());
            }
            throw new Exception("Platform used has no resources file.");
        }

        /// <summary>
        /// Get element class name based on running platform
        /// </summary>
        public static string GetClassName(PlatformClassName resName)
        {
            return GetFromElementResource(resName.ToString());
        }

        /// <summary>
        /// Get value returned from a native backdoor based on running platform
        /// </summary>
        public static string GetValueName(PlatformValueName resName)
        {
            return GetFromElementResource(resName.ToString());
        }

        /// <summary>
        /// Get native backdoor name based on running platform
        /// </summary>
        public static string GetInvokeName(PlatformInvokeName resName)
        {
            return GetFromElementResource(resName.ToString());
        }

        /// <summary>
        /// Execute some specific code only on a specific platform
        /// </summary>
        public static void OnPlatform(Platform platform, Action callback)
        {
            if (_platform == platform)
            {
                callback();
            }
        }

        /// <summary>
        /// Execute some specific code only on Android
        /// </summary>
        public static void OnAndroid(Action callback)
        {
            if (isAndroid)
            {
                callback();
            }
        }

        /// <summary>
        /// Execute some specific code only on IOS
        /// </summary>
        public static void OnIOS(Action callback)
        {
            if (isIos)
            {
                callback();
            }
        }

        /// <summary>
        /// Wait some ms on every platform
        /// </summary>
        public static void Wait(int msToWait)
        {
            System.Threading.Thread.Sleep(msToWait);
        }

        /// <summary>
        /// Wait some ms only on Android
        /// </summary>
        public static void WaitOnAndroid(int msToWait)
        {
            if (isAndroid)
            {
                Wait(msToWait);
            }
        }

        /// <summary>
        /// Wait some ms only on iOS
        /// </summary>
        public static void WaitOnIOS(int msToWait)
        {
            if (isIos)
            {
                Wait(msToWait);
            }
        }

        /// <summary>
        /// Wait some ms and after go back, only on Android
        /// </summary>
        public static void GoBackOnAndroid(int msToWait = 2000)
        {
            if (isAndroid)
            {
                Wait(msToWait);
                _app.Back();
            }
        }

        /// <summary>
        /// Get the translated string from a resx file
        /// </summary>
        public static string GetTranslatedResx(string resourceName)
        {
            return TextForTesting.ResourceManager.GetString($"{resourceName}_{_language}");
        }

        /// <summary>
        /// Wait for an element and throw an exception if the waiting takes longer than 20 seconds (time can be passed also as parameter)
        /// </summary>
        public static void WaitElement(Func<AppQuery, AppQuery> query, string elementName, int timeoutSeconds = DefaultQueryTimeoutSeconds)
        {
            _app.WaitForElement(query, "Timed out waiting for element " + elementName, new TimeSpan(0, 0, timeoutSeconds));
        }

        /// <summary>
        /// Wait for an element and throw an exception if the waiting takes longer than 20 seconds (time can be passed also as parameter)
        /// </summary>
        public static void WaitElement(string name, QueryType type = QueryType.Marked, int timeoutSeconds = DefaultQueryTimeoutSeconds)
        {
            switch (type)
            {
                case QueryType.Id:
                    _app.WaitForElement(x => x.Id(name), "Timed out waiting for element " + name, new TimeSpan(0, 0, timeoutSeconds));
                    break;
                case QueryType.Text:
                    _app.WaitForElement(x => x.Text(name), "Timed out waiting for element " + name, new TimeSpan(0, 0, timeoutSeconds));
                    break;
                case QueryType.Class:
                    _app.WaitForElement(x => x.Class(name), "Timed out waiting for element " + name, new TimeSpan(0, 0, timeoutSeconds));
                    break;
                case QueryType.Marked:
                    _app.WaitForElement(name, "Timed out waiting for element " + name, new TimeSpan(0, 0, timeoutSeconds));
                    break;
            }
        }

        /// <summary>
        /// Wait and tap an element and throw an exception if the waiting takes longer than 20 seconds (time can be passed also as parameter)
        /// </summary>
        public static void WaitAndTapElement(Func<AppQuery, AppQuery> query, string elementName, int timeoutSeconds = DefaultQueryTimeoutSeconds)
        {
            _app.WaitForElement(query, "Timed out waiting for element " + elementName, new TimeSpan(0, 0, timeoutSeconds));
            _app.Tap(query);
        }

        /// <summary>
        /// Wait and tap an element and throw an exception if the waiting takes longer than 20 seconds (time can be passed also as parameter)
        /// </summary>
        public static void WaitAndTapElement(string name, QueryType type = QueryType.Marked, int timeoutSeconds = DefaultQueryTimeoutSeconds)
        {
            switch (type)
            {
                case QueryType.Id:
                    _app.WaitForElement(x => x.Id(name), "Timed out waiting for element " + name, new TimeSpan(0, 0, timeoutSeconds));
                    _app.Tap(x => x.Id(name));
                    break;
                case QueryType.Text:
                    _app.WaitForElement(x => x.Text(name), "Timed out waiting for element " + name, new TimeSpan(0, 0, timeoutSeconds));
                    _app.Tap(x => x.Text(name));
                    break;
                case QueryType.Class:
                    _app.WaitForElement(x => x.Class(name), "Timed out waiting for element " + name, new TimeSpan(0, 0, timeoutSeconds));
                    _app.Tap(x => x.Class(name));
                    break;
                case QueryType.Marked:
                    _app.WaitForElement(name, "Timed out waiting for element " + name, new TimeSpan(0, 0, timeoutSeconds));
                    _app.Tap(name);
                    break;
            }
        }

        /// <summary>
        /// Take a screenshot if they are enabled
        /// </summary>
        public static void TakeScreenshot(string text)
        {
            if (ScreenshotEnabled)
            {
                _app.Screenshot(text);
            }
        }

        /// <summary>
        /// Convert a string 12h formato to 24h, only if the running os is Android, else it returns the same string.
        /// 9:00 AM -> 09:00
        /// 9:00 PM -> 21:00
        /// 09:00 PM -> Exception
        /// 9:00PM -> Exception
        /// 12:00 PM -> 12:00
        /// 12:00 AM -> 00:00
        /// </summary>
        /// <param name="time12HoursFormat"></param>
        /// <returns></returns>
        public static string Convert12HoursFormatoTo24HoursFormat_OnlyOnAndroid(string time12HoursFormat)
        {
            if (isAndroid)
            {
                bool isPM = time12HoursFormat.Substring(time12HoursFormat.Length - 2, 2) == "PM";
                int hoursLenght = time12HoursFormat.Length - 6;
                int hours12 = int.Parse(time12HoursFormat.Substring(0, hoursLenght));
                string hours24 = (hours12 + (isPM ? 12 : 0) % 24).ToString();
                string minutes = time12HoursFormat.Substring(hoursLenght + 1, 2);
                return (hours24.Length == 1 ? "0" : "") + hours24 + ":" + minutes;
            }
            return time12HoursFormat;
        }

        /// <summary>
        /// Execute a backdoor. It can return data that must be casted. 
        /// If the backdoor has no parameters it passes an empty string, to avoid problem with ios
        /// </summary>
        public static object ExecuteBackdoor(Backdoors backdoor, string parameter = "")
        {
            return _app.Invoke(GetBackdoorName(backdoor), parameter);
        }

        /// <summary>
        /// Check is an element is visible
        /// </summary>
        public static bool IsVisible(Func<AppQuery, AppQuery> query)
        {
            return _app.Query(query).Length > 0;
        }

        /// <summary>
        /// Check is an element is visible
        /// </summary>
        public static bool IsVisible(string marked)
        {
            return _app.Query(marked).Length > 0;
        }

        /// <summary>
        /// Check if the ONLY switch in view is checked
        /// </summary>
        public static bool Switch_IsChecked()
        {
            return _app.Query(x => x.Class(GetClassName(PlatformClassName.Switch)).Invoke(GetInvokeName(PlatformInvokeName.Switch_IsChecked)))[0].ToString() == GetValueName(PlatformValueName.Switch_CheckedValue);
        }

        /// <summary>
        /// Get the value of the ONLY switch in view
        /// </summary>
        public static string GetSwitchValue()
        {
            return _app.Query(x => x.Class(GetClassName(PlatformClassName.Switch)).Invoke(GetInvokeName(PlatformInvokeName.Switch_IsChecked)))[0].ToString();
        }

        /// <summary>
        /// Get the text of an element
        /// </summary>
        public static string GetText(string marked)
        {
            return _app.Query(marked)[0].Text;
        }

        /// <summary>
        /// Get the text of an element
        /// </summary>
        public static string GetText(Func<AppQuery, AppQuery> query)
        {
            return _app.Query(query)[0].Text;
        }

        /// <summary>
        /// Get resource string from a resource file based on platform
        /// </summary>
        private static string GetFromElementResource(string resName)
        {
            if (isAndroid)
            {
                return ElementAndroid.ResourceManager.GetString(resName);
            }
            if (isIos)
            {
                return ElementIOS.ResourceManager.GetString(resName);
            }
            throw new Exception("Platform used has no resources file.");
        }

        /// <summary>
        /// Return the android api that running on current device
        /// </summary>
        public static int GetAndroidApiVersion()
        {
            return int.Parse((string)ExecuteBackdoor(Backdoors.GetAndroidVersion));
        }

        /// <summary>
        /// Wait for at least one element and throw an exception if the waiting takes longer than 20 seconds (time can be passed also as parameter)
        /// It returns the selector visible
        /// </summary>
        public static Func<AppQuery, AppQuery> WaitElements(Func<AppQuery, AppQuery>[] queries, string collectionName, int timeoutSeconds = DefaultQueryTimeoutSeconds)
        {
            for (int tryCount = 0; tryCount < timeoutSeconds; tryCount++)
            {
                foreach (var query in queries)
                {
                    if (IsVisible(query))
                    {
                        return query;
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
            throw new Exception("Cannot wait collection " + collectionName);
        }
    }

    /// <summary>
    /// Mostly used types when execute app.Query
    /// </summary>
    public enum QueryType
    {
        Id,
        Text,
        Class,
        Marked
    }

    /// <summary>
    /// Enum used to get component class name based on platform from a resx file
    /// </summary>
    public enum PlatformClassName
    {
        Switch,
        TimePicker,
        Slider,
        ListView_Row
    }

    /// <summary>
    /// Enum used to get native backdoor name based on platform from a resx file
    /// </summary>
    public enum PlatformInvokeName
    {
        Switch_IsChecked
    }

    /// <summary>
    /// Enum used to get the value returned from native backdoor, based on platform from a resx file
    /// </summary>
    public enum PlatformValueName
    {
        Switch_CheckedValue,
        Switch_NotCheckedValue
    }

    /// <summary>
    /// Enum used to get custom backdoor name, based on platform from a resx file
    /// </summary>
    public enum Backdoors
    {
        /// <summary>
        /// Set test mode flag. If this is set to false the other backdoors will not work
        /// </summary>
        SetTestMode,
        /// <summary>
        /// Set the qrcode that will be return from the scan without using the camera
        /// </summary>
        SetTestScannedQrCode,
        /// <summary>
        /// Mock the system time
        /// </summary>
        SetTimeNow_SystemUtility,
        /// <summary>
        /// Set a flag that indicates if device will receive local notifications
        /// </summary>
        SetSendNotifications,
        /// <summary>
        /// Get checked items in shopping list. This string is already formatted for test usage
        /// </summary>
        GetShoppingListCheckBoxStatus,
        /// <summary>
        /// It allows the test to start from a situation where the QR is already scanned. 
        /// ATTENTION: This method has to be invoked after the button "Tap here to get started" has been pressed
        /// </summary>
        SimulateScannedQrCode,
        /// <summary>
        /// Get 'To Do' list from 'All my daily task'
        /// </summary>
        GetToDoList,
        /// <summary>
        /// Get 'Done' list from 'All my daily task'
        /// </summary>
        GetDoneList,
        /// <summary>
        /// Get the struct direction, during the strut adjustment operation, image source
        /// </summary>
        GetSelectedStrutDirectionImage,
        /// <summary>
        /// Get the strut background, during the strut adjustment operation, image source
        /// </summary>
        GetSelectedStrutBackgroundImageName,
        /// <summary>
        /// Return android api version. This works only on Android.
        /// </summary>
        GetAndroidVersion,
        /// <summary>
        /// Get a list of badges visualized in strut adj page
        /// </summary>
        GetBadgeStrutsAdj,
        /// <summary>
        /// Get a list of frames visualized in strut adj page
        /// </summary>
        GetFrameStrutsAdj
    }

    public class AnonymousTable
    {
        public string Nickname { get; set; }
        public string Time { get; set; }
    }

    public class NormalTable
    {
        public string Nickname { get; set; }
        public string TimePinSite { get; set; }
        public string MyText { get; set; }
        public string TimeInsigth { get; set; }
        public string GoalStat { get; set; }
        public string InsightsStat { get; set; }
    }
}
