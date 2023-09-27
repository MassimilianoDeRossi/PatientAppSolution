using System.Diagnostics;
using PatientApp.ApplicationObjects;
using Xamarin.Forms;
using PatientApp.ViewModels;
using DLToolkit.Forms.Controls;

namespace PatientApp
{
    //    public enum AppStatusEnum
    //    {
    //        Unknown,
    //        LoggedOut,
    //        ConnectionMissing,
    //        Active,
    //        Locked,
    //    }

    /// <summary>
    ///  Main applicaton class
    /// </summary>
    public partial class App : Application
    {
        private static bool _doNotDisturbMode = false;

        /// <summary>
        /// Indicates if user is setting data in user settings wizard. Used to avoid sync settings that cause a wrong scheduling pin site care notifications.
        /// </summary>
        public static bool IsWizardUserSettingsActive { get; set; }

        /// <summary>
        /// A model for test support purposes
        /// </summary>
        public static TestSupport.TestModel TestModel { get; set; }

        /// <summary>
        /// Indicates if user is setting data in user settings wizard. Used to avoid sync settings that cause a wrong scheduling pin site care notifications.
        /// </summary>
        public static bool IsCertificateChecked { get; set; } = false;

        /// <summary>
        /// Screen width in DP (calculated in the native layer)
        /// </summary>
        public static int ScreenWidth { get; set; }
        /// <summary>
        /// Screen height in DP (calculated in the native layer)
        /// </summary>
        public static int ScreenHeight { get; set; }

        /// <summary>
        /// Item identifier the push notification refer to
        /// </summary>
        public static int? PushItemId { get; set; } = null;

        /// <summary>
        /// Notification token to be passed to the push notification service (APN for ios for example)
        /// </summary>
        public static string PushNotificationToken { get; set; } = null;

        /// <summary>
        /// The version of running build taken from app manifest
        /// </summary>
        public static string RuntimVersion = null;

        /// <summary>
        /// Colors of page toolbar (used by custom renderer in native layer)
        /// </summary>
        public static Color ToolbarColor = Color.FromRgb(73, 73, 72);
        public static Color ToolbarTintColor = Color.Red;
        public static Color ToolbarSelectedImageTintColor = Color.FromRgb(0, 94, 161);

        /// <summary>
        /// Global instance of viewmodels' factory 
        /// </summary>
        public static ViewModelLocator ViewModelLocator => new ViewModelLocator();

        /// <summary>
        /// The application Navigation Manager
        /// </summary>
        private static NavigationController _navigationController;
        public static NavigationController NavigationController => _navigationController = _navigationController ?? new NavigationController();

        public App(AppSetup setup)
        {
            // Make sure it doesn't get stripped away by the linker

            InitializeComponent();

            // Init FlowListView control plugin
            FlowListView.Init();

            // Create AutoFac container and register services
            AppContainer.Container = setup.CreateContainer();
            ViewModelLocator.RegisterServices(AppContainer.Container);

            TestModel = new TestSupport.TestModel();



#if ENABLE_TEST_CLOUD
            TestModel.TestModeOn = true;
            TestModel.MockedScannedQrCode = new Networking.PrescriptionsMockedIndices().QrCode1;
#else

            TestModel.MockedScannedQrCode = new Networking.PrescriptionsMockedIndices().QrCodeDev;
            TestModel.TestModeOn = Debugger.IsAttached;
#endif

            // Initialize navigation
            NavigationController.InitializeNavigation();
        }

        protected override void OnStart()
        {
            //Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Broadcast notification of resumed state
            MessagingCenter.Send<App>(this, Messaging.Messages.APP_RESUMED);
        }

        /// <summary>
        /// Set the main page of the app
        /// </summary>
        /// <param name="page"></param>
        public static void SetMainPage(Page page)
        {
            Current.MainPage = page;
        }

        /// <summary>
        /// Application scoped state used to disable user interaction during some operations (like strut adjustments)
        /// </summary>
        public static void SetDoNotDisturbMode(bool mode)
        {
            _doNotDisturbMode = mode;
            if (mode)
                MessagingCenter.Send<Application>(App.Current, Messaging.Messages.APP_DONOTDISTURB_ON);
            else
                MessagingCenter.Send<Application>(App.Current, Messaging.Messages.APP_DONOTDISTURB_OFF);
        }

    }
}
