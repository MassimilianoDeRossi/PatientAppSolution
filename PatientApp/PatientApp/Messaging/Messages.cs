namespace PatientApp.Messaging
{
    /// <summary>
    /// Broadcast messages used in MVVM communication
    /// </summary>
    public class Messages
    {
        public const string APP_SUSPENDED = "APP_SUSPENDED_MESSAGE";
        public const string APP_RESUMED = "APP_RESUMED_MESSAGE";
        public const string APP_DONOTDISTURB_ON = "APP_DONOTDISTURB_ON";
        public const string APP_DONOTDISTURB_OFF = "APP_DONOTDISTURB_OFF";
        public const string APP_STATE_CHANGED = "APP_STATE_CHANGED";

        public const string VIEW_INIT_MESSAGE = "VIEW_INIT_MESSAGE";
        public const string VIEW_APPEARING_MESSAGE = "VIEW_APPEARING_MESSAGE";
        public const string VIEW_DISAPPEARING_MESSAGE = "VIEW_DISAPPEARING_MESSAGE";
        public const string ANDROID_BACKBUTTON_PRESSED = "ANDROID_BACKBUTTON_PRESSED";

        public const string USER_LOGGED_IN = "USER_LOGGED_IN_MESSAGE";
        public const string USER_LOGGED_OUT = "USER_LOGGED_OUT_MESSAGE";

        public const string PRESCRIPTION_CODE_SCANNED = "PRESCRIPTION_CODE_SCANNED_MESSAGE";
        public const string SYNC_SETTINGS_REQUEST = "SYNC_SETTINGS_REQUEST";

        public const string SURGEON_CONTACTS_UPDATED = "SURGEON_CONTACTS_UPDATED";
    }
}
