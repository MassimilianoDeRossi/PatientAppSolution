using System;
using System.Collections.Generic;
using System.Linq;

using Plugin.Settings;
using Plugin.Settings.Abstractions;

using PatientApp.Utilities;
using Newtonsoft.Json;
using Xamarin.Forms;
using PatientApp.Services;

namespace PatientApp.Settings
{
    /// <summary>
    /// Global Application Settings manager
    /// </summary>
    public class AppSettings
    {
        public const string APP_SETTINGS_KEY = "APP_SETTINGS";
        public static readonly TimeSpan PinSiteCareTimeDefaultValue = new TimeSpan(9, 0, 0);
        public static readonly TimeSpan InsightTimeDefaultValue = new TimeSpan(9, 0, 0);

        private AppSettings()
        {

        }

        private static ISettings Settings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        private static AppSettings _instance = null;

        /// <summary>
        /// Return the instance of AppSettings saved in application storage
        /// </summary>
        public static AppSettings Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Unique application instance identifier
        /// </summary>
        [JsonProperty("ApplicationInstanceId")]
        public Guid ApplicationInstanceId { get; protected set; }

        /// <summary>
        /// Unique application instance identifier
        /// </summary>
        [JsonProperty("CurrentLanguageCode")]
        public string CurrentLanguageCode { get; protected set; }

        /// <summary>
        /// Legal Terms Accepted setting
        /// </summary>
        [JsonProperty("AreLegalTermsAccepted")]
        public bool AreLegalTermsAccepted { get; protected set; }

        /// <summary>
        /// Legal Terms Accepted setting
        /// </summary>
        [JsonProperty("LegalTermsAcceptedVersion")]
        public string LegalTermsAcceptedVersion { get; protected set; }

        /// <summary>
        /// Unique patient identifier
        /// </summary>
        [JsonProperty("PatientId")]
        public Guid PatientId { get; protected set; }

        /// <summary>
        /// Unique case identifier
        /// </summary>
        [JsonProperty("CaseId")]
        public Guid CaseId { get; protected set; }

        /// <summary>
        /// User personal goal enable/disable setting
        /// </summary>
        [JsonProperty("IsLoggedIn")]
        public bool IsLoggedIn { get; protected set; }

        /// <summary>
        /// user nickname 
        /// </summary>
        [JsonProperty("Nickname")]
        public string Nickname { get; protected set; }

        /// <summary>
        /// The absolute path of selected user profile image in the device storage
        /// </summary>
        [JsonProperty("ProfileImagePath")]
        public string ProfileImagePath { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("MoodIndex")]
        public int? MoodIndex { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("LastMoodDateTime")]
        public System.DateTime? LastMoodDateTime { get; protected set; }

        /// <summary>
        /// User personal goal enable/disable setting
        /// </summary>
        [JsonProperty("IsGoalEnabled")]
        public bool IsGoalEnabled { get; protected set; }

        /// <summary>
        /// User goal disabled setting
        /// </summary>
        [JsonProperty("PersonalGoal")]
        public string PersonalGoal { get; protected set; }

        /// <summary>
        /// User insight messages enable/disable setting
        /// </summary>
        [JsonProperty("IsInsightEnabled")]
        public bool IsInsightEnabled { get; protected set; }

        /// <summary>
        /// User selected insight message notification time 
        /// </summary>
        [JsonProperty("InsightTime")]
        public TimeSpan InsightTime { get; protected set; }

        /// <summary>
        /// Last datetime of succesfullly completed sync to server data
        /// </summary>
        [JsonProperty("SyncLastDateTime")]
        public DateTime? SyncLastDateTime { get; protected set; }

        /// <summary>
        /// Last datetime of reminders update
        /// </summary>
        [JsonProperty("ReminderLastUpdate")]
        public DateTime? ReminderLastUpdate { get; protected set; }

        /// <summary>
        /// Last datetime of notification scheduling
        /// </summary>
        [JsonProperty("SchedulingLastDateTime")]
        public DateTime? SchedulingLastDateTime { get; protected set; }

        /// <summary>
        /// Last datetime of succesfullly settings upload to server 
        /// </summary>
        [JsonProperty("SettingsLastUploadDateTime")]
        public DateTime? SettingsLastUploadDateTime { get; protected set; }

        /// <summary>
        /// Last datetime of succesfullly settings upload to server 
        /// </summary>
        [JsonProperty("SettingsLastUpdateDateTime")]
        public DateTime SettingsLastUpdateDateTime { get; protected set; }

        /// <summary>
        /// Flag that track enable disable of pin site care reminders notification choosed by the surgeon.
        /// </summary>
        [JsonProperty("PinSiteCareEnabled")]
        public bool PinSiteCareEnabled { get; protected set; }

        /// <summary>
        /// pin site care starting datetime.
        /// </summary>
        [JsonProperty("PinSiteCareStartDate")]
        public DateTime? PinSiteCareStartDate { get; protected set; }

        /// <summary>
        /// User selected pin site care notification time 
        /// </summary>
        [JsonProperty("PinSiteCareTime")]
        public TimeSpan PinSiteCareTime { get; protected set; }

        /// <summary>
        /// Enabled days of week for pin site care notification choosed by the surgeons on myhexplanportal.
        /// </summary>
        [JsonProperty("PinSiteCareDaysOfWeekNotification")]
        public bool[] PinSiteCareDaysOfWeekNotification { get; protected set; }

        /// <summary>
        /// Legal Terms Accepted setting
        /// </summary>
        [JsonProperty("ShowStrutsSkippedWarning")]
        public bool ShowStrutsSkippedWarning { get; protected set; }

        /// <summary>
        /// Legal Terms Accepted setting
        /// </summary>
        [JsonProperty("HasSyncPending")]
        public bool HasSyncPending { get; protected set; }

        [JsonProperty("LastWakeUpDateTime")]
        public DateTime? LastWakeUpDateTime { get; protected set; }

        /// <summary>
        /// Signing certificate content 
        /// </summary>
        [JsonProperty("SigningCertificateContent")]
        public byte[] SigningCertificateContent { get; protected set; }

        [JsonProperty("LastSigningCertificateUpdate")]
        public DateTime? LastSigningCertificateUpdate { get; protected set; }

        [JsonProperty("DatabaseVersion")]
        public int? DatabaseVersion { get; protected set; }

        /// <summary>
        /// Save values to application storage
        /// </summary>
        private void Save()
        {
            SaveSerializedObject(APP_SETTINGS_KEY, this);
        }

        /// <summary>
        /// Initialize Application Settings to default values if not already done
        /// </summary>
        public static void Initialize()
        {
            _instance = GetSerializedObject<AppSettings>(APP_SETTINGS_KEY);
            if (_instance == null || _instance.ApplicationInstanceId == Guid.Empty)
            {
                // First run: Create new application settings with a new ApplicationInstanceId and default settings
                _instance = new AppSettings()
                {
                    AreLegalTermsAccepted = false,
                    PinSiteCareTime = PinSiteCareTimeDefaultValue,
                    InsightTime = InsightTimeDefaultValue,
                    IsGoalEnabled = false,
                    IsInsightEnabled = false,
                    IsLoggedIn = false,
                    ShowStrutsSkippedWarning = false,
                    HasSyncPending = false,
                    MoodIndex = null,
                    CurrentLanguageCode = null,
                    ApplicationInstanceId = Guid.NewGuid(),
                    DatabaseVersion = 2,
                };
                // Store the newly created settings
                _instance.Save();
            }
        }

        /// <summary>
        /// Force reset Application Setttings to initial values
        /// </summary>
        public void ResetAll()
        {
            _instance = null;
            Settings.AddOrUpdateValue(APP_SETTINGS_KEY, null);
            // Force recreation of default values            
            Initialize();
        }

        /// <summary>
        /// Get REST Api login username
        /// </summary>
        /// <returns></returns>
        public string GetApiUserName()
        {
            var token = String.IsNullOrEmpty(App.PushNotificationToken) ? "1d2b5f9f751255f00b7eb8e1e71e43ca192e06c20c7d904f7721ca04c9b35f4b" : App.PushNotificationToken;
            int platformId = (int)(Device.RuntimePlatform == Device.Android ? NotificationPlatformType.Gcm : NotificationPlatformType.Apns);
            var user = string.Concat(this.PatientId, "|", this.CaseId, "|", token, "|", platformId.ToString());
            return user;
        }

        //TODO: da rimuovere
        public string GetToken()
        {
          return String.IsNullOrEmpty(App.PushNotificationToken) ? "1d2b5f9f751255f00b7eb8e1e71e43ca192e06c20c7d904f7721ca04c9b35f4b" : App.PushNotificationToken;
        }

        /// <summary>
        /// Get REST Api login password
        /// </summary>
        /// <returns></returns>
        public string GetApiPassword()
        {
            var password = DependencyService.Get<ICryptoService>().Encrypt(this.ApplicationInstanceId.ToString());
            return password;
        }

        /// <summary>
        /// Reset settings to anonymous mode values
        /// </summary>
        public static void ResetToAnonymous()
        {
            _instance.IsLoggedIn = false;
            _instance.LastMoodDateTime = null;
            _instance.LastWakeUpDateTime = null;
            _instance.MoodIndex = null;
            _instance.PinSiteCareEnabled = false;
            _instance.PinSiteCareDaysOfWeekNotification = null;
            _instance.PinSiteCareStartDate = null;
            _instance.ReminderLastUpdate = null;
            _instance.SchedulingLastDateTime = null;
            _instance.SettingsLastUpdateDateTime = DateTime.MinValue;
            _instance.SettingsLastUploadDateTime = null;
            _instance.ShowStrutsSkippedWarning = false;
            _instance.IsGoalEnabled = false;
            _instance.PersonalGoal = null;
            _instance.IsInsightEnabled = false;
            _instance.SyncLastDateTime = null;
            _instance.CurrentLanguageCode = null;
            _instance.LastSigningCertificateUpdate = null;
            _instance.SigningCertificateContent = null;

            if (System.Diagnostics.Debugger.IsAttached)
                _instance.ApplicationInstanceId = Guid.Empty;

            _instance.Save();
        }


        /// <summary>
        /// Set and save the selected Language Code
        /// </summary>
        /// <param name="langCode"></param>
        public static void SetLanguageCode(string langCode)
        {
            _instance.CurrentLanguageCode = langCode;
            _instance.Save();
        }

        /// <summary>
        /// Set and savew the value of IsLoggedIn setting
        /// </summary>
        /// <param name="isLoggedIn"></param>
        public static void SetLoginState(bool isLoggedIn)
        {
            _instance.IsLoggedIn = isLoggedIn;
            _instance.Save();
        }

        /// <summary>
        /// Set and save the local database version
        /// </summary>
        /// <param name="langCode"></param>
        public static void SetDatabaseVersion(int version)
        {
            _instance.DatabaseVersion = version;
            _instance.Save();
        }

        /// <summary>
        /// Set and save the value of selected Mood Index at a certain date
        /// </summary>
        /// <param name="index"></param>
        /// <param name="atDateTime"></param>
        public static void SetMoodIndex(int? index, DateTime? atDateTime = null)
        {
            _instance.MoodIndex = index;
            _instance.LastMoodDateTime = atDateTime ?? DateTime.Now;
            _instance.Save();
        }

        /// <summary>
        /// Set and save patient case data
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="caseId"></param>
        public static void SetPatientInfo(Guid patientId, Guid caseId)
        {
            _instance.PatientId = patientId;
            _instance.CaseId = caseId;
            _instance.Save();
        }

        /// <summary>
        /// Mark Legal Term as accepted for current application version
        /// </summary>
        public static void SetLegalTermsAccepted()
        {
            _instance.AreLegalTermsAccepted = true;
            _instance.LegalTermsAcceptedVersion = App.RuntimVersion;
            _instance.Save();
        }

        /// <summary>
        /// Set and save user nickname and profile image 
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="profileImagePath"></param>
        public static void SetUserProfile(string nickname, string profileImagePath)
        {
            _instance.Nickname = nickname;
            _instance.ProfileImagePath = profileImagePath;
            _instance.Save();
        }

        /// <summary>
        /// Set and save pin site care settings
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="time"></param>
        /// <param name="refDateTime"></param>
        public static void SetPinSiteCare(bool enabled, TimeSpan time, DateTime refDateTime)
        {
            _instance.PinSiteCareEnabled = enabled;
            _instance.PinSiteCareTime = time;
            _instance.SettingsLastUpdateDateTime = refDateTime;
            _instance.Save();
        }

        /// <summary>
        /// Set and save Personal Goal settings
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="personalGoal"></param>
        public static void SetPersonalGoal(bool enabled, string personalGoal)
        {
            _instance.IsGoalEnabled = enabled;
            _instance.PersonalGoal = personalGoal;
            _instance.Save();
        }

        /// <summary>
        /// Set and save Insight settings (Motivational Messages)
        /// </summary>
        /// <param name="isInsightEnabled"></param>
        /// <param name="insightTime"></param>
        public static void SetInsight(bool isInsightEnabled, TimeSpan insightTime)
        {
            _instance.IsInsightEnabled = isInsightEnabled;
            _instance.InsightTime = insightTime;
            _instance.Save();
        }

        /// <summary>
        /// Set and save all main settings
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="profileImagePath"></param>
        /// <param name="pinSiteCareTime"></param>
        /// <param name="isInsightEnabled"></param>
        /// <param name="insightTime"></param>
        /// <param name="isGoalEnabled"></param>
        /// <param name="personalGoal"></param>
        /// <param name="refDateTime"></param>
        public static void SetAllSettings(string nickname, string profileImagePath, TimeSpan pinSiteCareTime, bool isInsightEnabled, TimeSpan insightTime, bool isGoalEnabled, string personalGoal, DateTime refDateTime)
        {
            _instance.Nickname = nickname;
            _instance.ProfileImagePath = profileImagePath;
            _instance.PinSiteCareTime = pinSiteCareTime;
            _instance.IsInsightEnabled = isInsightEnabled;
            _instance.InsightTime = insightTime;
            _instance.IsGoalEnabled = isGoalEnabled;
            _instance.PersonalGoal = personalGoal;
            _instance.SettingsLastUpdateDateTime = refDateTime;
            _instance.Save();
        }

        /// <summary>
        /// Set and save portal settings if something has changed since last save
        /// </summary>
        /// <param name="pinSitecareEnabled"></param>
        /// <param name="startDate"></param>
        /// <param name="pinSiteCareDaysOfWeekNotification"></param>
        /// <returns>True if something has changed</returns>
        public static bool SetPortalSettingsIfChanged(bool? pinSitecareEnabled, DateTime? startDate, IList<bool?> pinSiteCareDaysOfWeekNotification)
        {
            bool changed = false;

            if (pinSitecareEnabled.HasValue && pinSitecareEnabled.Value != Instance.PinSiteCareEnabled)
            {
                changed = true;
                Instance.PinSiteCareEnabled = pinSitecareEnabled.Value;
            }

            if ((startDate.HasValue && !Instance.PinSiteCareStartDate.HasValue)
              || (startDate.HasValue && Instance.PinSiteCareStartDate.HasValue && startDate.Value != Instance.PinSiteCareStartDate.Value))
            {
                Instance.PinSiteCareStartDate = startDate.Value;
                changed = true;
            }

            if (pinSiteCareDaysOfWeekNotification != null)
            {
                // Are comparable?
                if (Instance.PinSiteCareDaysOfWeekNotification != null && Instance.PinSiteCareDaysOfWeekNotification.Length == pinSiteCareDaysOfWeekNotification.Count)
                {
                    // check if something has changed
                    for (int index = 0; index < pinSiteCareDaysOfWeekNotification.Count; index++)
                    {
                        if (pinSiteCareDaysOfWeekNotification[index].HasValue && pinSiteCareDaysOfWeekNotification[index].Value != Instance.PinSiteCareDaysOfWeekNotification[index])
                        {
                            changed = true;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    changed = true;
                }

                if (changed)
                    Instance.PinSiteCareDaysOfWeekNotification = pinSiteCareDaysOfWeekNotification.Select(b => b.HasValue ? b.Value : false).ToArray();
            }

            if (changed)
                Instance.Save();
            return changed;
        }

        /// <summary>
        /// Set and save last datetime of prescriptions sync
        /// </summary>
        /// <param name="dt"></param>
        public static void SetSyncLastDateTime(DateTime? dt)
        {
            _instance.SyncLastDateTime = dt;
            _instance.Save();
        }

        /// <summary>
        /// Set and save last datetime a reminder has been updated
        /// </summary>
        /// <param name="dt"></param>
        public static void SetReminderLastUpdate(DateTime? dt)
        {
            _instance.ReminderLastUpdate = dt;
            _instance.Save();
        }

        /// <summary>
        /// Set and save last scheduling datetime
        /// </summary>
        /// <param name="dt"></param>
        public static void SetSchedulingLastDateTime(DateTime? dt)
        {
            _instance.SchedulingLastDateTime = dt;
            _instance.Save();
        }

        /// <summary>
        /// Set and save last settings upload datetime
        /// </summary>
        /// <param name="dt"></param>
        public static void SetSettingsLastUploadDateTime(DateTime? dt)
        {
            _instance.SettingsLastUploadDateTime = dt;
            _instance.Save();
        }

        /// <summary>
        /// Set and save a flag to enable a warning in strut adjustment view
        /// </summary>
        /// <param name="show"></param>
        public static void SetShowStrutsSkippedWarning(bool show)
        {
            _instance.ShowStrutsSkippedWarning = show;
            _instance.Save();
        }

        /// <summary>
        /// Set and save a flag to inform that a prescription sync in needed
        /// </summary>
        /// <param name="pending"></param>
        public static void SetHasSyncPending(bool pending)
        {
            _instance.HasSyncPending = pending;
            _instance.Save();
        }

        /// <summary>
        /// Set and save the last datetime of received wakeup notification 
        /// </summary>
        /// <param name="dt"></param>
        public static void SetLastWakeUpDateTime(DateTime? dt)
        {
            _instance.LastWakeUpDateTime = dt;
            _instance.Save();
        }

        /// <summary>
        /// Set signing certificate content and date of last update
        /// </summary>
        /// <param name="pending"></param>
        public static void SetSigningCertificate(byte[] content, DateTime dt)
        {
            _instance.SigningCertificateContent = content;
            _instance.LastSigningCertificateUpdate = dt;
            _instance.Save();
        }


        /// <summary>
        /// Get a serialized object stored in settings 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        private static T GetSerializedObject<T>(string key)
        {
            var stringValue = Settings.GetValueOrDefault(key, null);
            if (!string.IsNullOrEmpty(stringValue))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringValue);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Serialize and save and object in settings
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        private static void SaveSerializedObject(string key, object obj)
        {
            var stringValue = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Settings.AddOrUpdateValue(key, stringValue);
        }

    }

}
