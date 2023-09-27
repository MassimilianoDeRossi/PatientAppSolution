using System;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Connectivity;

using PatientApp.Helpers;
using PatientApp.Interfaces;
using PatientApp.Views;
using PatientApp.Services;
using PatientApp.DataModel.SqlEntities;
using PatientApp.Settings;
using MyHexPlanProxies.Models;
using Newtonsoft.Json;
using PatientApp.Utilities;

namespace PatientApp.ViewModels
{
    public enum AppStateEnum
    {
        Unknown,
        Anonymous,
        Synced,
        SyncPending,
        Outdated
    }

    /// <summary>
    /// Main viewmodel. Manage all principal application features, including update background threars
    /// </summary>
    public class HomeViewModel : BaseViewModel, ILocalNotificationListener, IPushNotificationListener
    {
        private Timer lowPriorityCheckTimer = null;
        private Timer hiPriorityCheckTimer = null;
        private bool disableUI = false;
        string _supportUrlAnonymous = "", _supportUrlNormal = "";

        public Command AcceptTermsCommand { get; set; }


        private AppStateEnum _appState = AppStateEnum.Unknown;
        /// <summary>
        /// Application current state 
        /// </summary>
        public AppStateEnum AppState
        {
            get { return _appState; }
            set { SetProperty(ref _appState, value); }
        }

        /// <summary>
        /// Flag for testmode modality
        /// </summary>
        public bool IsTestMode
        {
            get { return App.TestModel.TestModeOn; }
        }

        bool _isConnectionAvailable = false;
        /// <summary>
        /// Network data connection availability flag
        /// </summary>
        public bool IsConnectionAvailable
        {
            get { return _isConnectionAvailable; }
            set { SetProperty(ref _isConnectionAvailable, value); }
        }

        DateTime _displayDate = DateTime.Today;
        /// <summary>
        /// TheDate displayed in home view
        /// </summary>
        public DateTime DisplayDate
        {
            get { return _displayDate; }
            set { SetProperty(ref _displayDate, value); }
        }

        /// <summary>
        /// Strut adjustment feature enabled/disabled flag
        /// </summary>
        public bool IsStrutAdjustmentEnabled
        {
            get { return IsLoggedIn && !AppSettings.Instance.HasSyncPending; }
        }

        private bool _hasStrutAdjustmentAlert = false;
        /// <summary>
        /// A flag indicating the presence of an alert on strut adjustment
        /// </summary>
        public bool HasStrutAdjustmentAlert
        {
            get { return _hasStrutAdjustmentAlert; }
            set { SetProperty(ref _hasStrutAdjustmentAlert, value); }
        }

        private bool _hasPinSiteCareAlert = false;
        /// <summary>
        /// A flag indicating the presence of an alert on pin site care
        /// </summary>
        public bool HasPinSiteCareAlert
        {
            get { return _hasPinSiteCareAlert; }
            set
            {
                SetProperty(ref _hasPinSiteCareAlert, value);
            }
        }

        private bool _hasPrescriptionAlert = false;
        /// <summary>
        /// A flag indicating the presence of an alert on prescriptions
        /// </summary>
        public bool HasPrescriptionAlert
        {
            get { return _hasPrescriptionAlert; }
            set { SetProperty(ref _hasPrescriptionAlert, value); }
        }

        private bool _hasNotUpToDateAlert = false;
        /// <summary>
        /// A flag indicating that a sync is needed
        /// </summary>
        public bool HasNotUpToDateAlert
        {
            get { return _hasNotUpToDateAlert; }
            set { SetProperty(ref _hasNotUpToDateAlert, value); }
        }

        string _legalTermsAndPrivacyText = string.Empty;
        /// <summary>
        /// Legal terms html text content
        /// </summary>
        public string LegalTermsAndPrivacyText
        {
            get { return _legalTermsAndPrivacyText; }
            set { SetProperty(ref _legalTermsAndPrivacyText, value); }
        }

        /// <summary>
        /// Application installation unique identifier
        /// </summary>
        string _applicationInstanceId = string.Empty;
        public string ApplicationInstanceId
        {
            get { return _applicationInstanceId; }
            set { SetProperty(ref _applicationInstanceId, value); }
        }

        private bool _isLanguageSelectionEnabled;
        public bool IsLanguageSelectionEnabled
        {
            get { return _isLanguageSelectionEnabled; }
            set { SetProperty(ref _isLanguageSelectionEnabled, value); }
        }


        /// <summary>
        /// Url of support web page, based on user Login state
        /// </summary>
        public string SupportUrl
        {
            get
            {
                var url = IsLoggedIn ? _supportUrlNormal : _supportUrlAnonymous;

                if (IsLanguageSelectionEnabled)
                {
                    // Build the support url according to the selected language
                    url = string.Format(url, Localization.LocalizationManager.GetCurrentLanguageCode().Substring(0, 2).ToLower());
                }
                else
                {
                    url = string.Format(url, string.Empty);
                }

                return url;
            }
        }

        /// <summary>
        /// Used to show/hide the "all my daily tasks" button on the view
        /// The button is visible if user is logged in and the current time is after 10AM
        /// </summary>
        public bool CanSetMood => System.Diagnostics.Debugger.IsAttached || (IsLoggedIn && _sysUtility.Now.Hour >= 10);

        public string MoodText => MoodManager.GetMoodAtDateTime(_sysUtility.Now);


        public HomeViewModel(ILocalDatabaseService dbService, IApiClient apiClient, ISystemUtility sysUtility) : base(dbService, apiClient, sysUtility)
        {
            _supportUrlAnonymous = PCLAppConfig.ConfigurationManager.AppSettings["SupportUrlAnonymous"];
            _supportUrlNormal = PCLAppConfig.ConfigurationManager.AppSettings["SupportUrlNormal"];
            IsLanguageSelectionEnabled = PCLAppConfig.ConfigurationManager.AppSettings["IsLanguageSelectionEnabled"] == "1";

            bool forceCreate = false;
            if (dbService.CreateTablesIfNotExists(forceCreate))
            {
            }

            if (!dbService.CheckIntegrity())
            {
                if (App.TestModel.TestModeOn)
                    App.Current.MainPage.DisplayAlert("Database Integrity Check", "Database corruption detected", "OK");
                else
                    AppLoggerHelper.LogEvent("Database Integrity Check", "Check failed - Database corruption detected", TraceLevel.Error);

            }

            ApplicationInstanceId = Settings.AppSettings.Instance.ApplicationInstanceId.ToString();

            AcceptTermsCommand = new Command(AcceptTermsCommandExecute);

            CheckConnection();

            // Prepare threads for period updates (hi and low priority)
            hiPriorityCheckTimer = new Timer(HiPriorityCheckCallback, null, 0, 5000);
            lowPriorityCheckTimer = new Timer(LowPriorityCheckCallback, null, 0, 60000);

            // Refresh all on first run 
            RefreshAlerts();
            HiPriorityCheckCallback(null);
            LowPriorityCheckCallback(null);

            MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, (page) =>
            {
                // Refresh view on appearing
                if (page is HomePage)
                {
                    // Force update because language could be changed
                    this.DisplayDate = DateTime.MinValue;
                    this.DisplayDate = _sysUtility.Now.Date;

                    OnPropertyChanged(nameof(IsLoggedIn));
                    OnPropertyChanged(nameof(IsStrutAdjustmentEnabled));
                    OnPropertyChanged(nameof(MoodText));
                    OnPropertyChanged(nameof(IsTestMode));

                    RefreshAlerts();
                }
                else if (page is SupportPage)
                {
                    if (IsConnectionAvailable)
                        OnPropertyChanged(nameof(SupportUrl));
                    else
                    {
                        App.Current.MainPage.DisplayAlert(Resources.PatientApp.ErrorTitle, Resources.PatientApp.ErrorConnectionMissing, Resources.PatientApp.BtnOK);
                        App.NavigationController.NavigateToHome();
                    }
                }
                else if (page is LegalTermsPage || page is PrivacyAndLegalTermsPage)
                {
                    UpdateLegalTermsLocalizedText();
                }
            });

            MessagingCenter.Subscribe<BaseViewModel>(this, Messaging.Messages.SYNC_SETTINGS_REQUEST, (vm) =>
            {
                SyncSettings();
            });

            MessagingCenter.Subscribe<Application>(this, Messaging.Messages.APP_DONOTDISTURB_ON, (app) =>
            {
                // Disable user interface when app enter in Do not disturb mode
                disableUI = true;
            });
            MessagingCenter.Subscribe<Application>(this, Messaging.Messages.APP_DONOTDISTURB_OFF, (app) =>
            {
                // Enable user interface when app exit from Do not disturb mode
                disableUI = false;
            });

            this.PropertyChanged += (s, e) =>
              {
                  if (e.PropertyName == nameof(this.AppState))
                  {
                      // Save in log history
                      var historyItem = new LogHistoryItem()
                      {
                          LocalEntityId = null,
                          ExpectedDateTime = null,
                          EventDateTime = _sysUtility.Now,
                          ItemType = LogHistoryItem.ItemTypeEnum.AppStateChanged,
                          Description = string.Format("App state changed to {0}", this.AppState.ToString()),
                          ServerEntityId = null,
                      };
                      _dbService.SaveHistoryLogItem(historyItem);
                  }
              };

            // Init and subscribe push notifications
            DependencyService.Get<INotificationManager>().Initialize(this, this);
        }

        private void AcceptTermsCommandExecute()
        {
            Settings.AppSettings.SetLegalTermsAccepted();
            App.NavigationController.NavigateToHome();
        }


        private bool _isHiChecking = false;
        private void HiPriorityCheckCallback(object state)
        {
            if (_isHiChecking)
                return;

            _isHiChecking = true;

            this.DisplayDate = _sysUtility.Now.Date;

            CheckConnection();
            // Update visibility flag
            OnPropertyChanged(nameof(CanSetMood));

            //// Check for pending sync 
            if (AppSettings.Instance.HasSyncPending && IsConnectionAvailable && !disableUI)
                SyncPrescriptions();

            RefreshAlerts();
            _isHiChecking = false;
        }

        private bool _isLowChecking = false;

        /// <summary>
        /// Periodically check for remote update availability 
        /// and performs local scheduled updates
        /// </summary>
        /// <param name="state"></param>
        private void LowPriorityCheckCallback(object state)
        {
            if (_isLowChecking)
                return;

            if (!IsConnectionAvailable)
                return;

            _isLowChecking = true;

            if (!disableUI)
            {
                UploadSettingsIfNeeded();
                RescheduleRemindersIfNeeded();
                SyncSettings();
                SyncPrescriptions();
            }

            // Check for signing certificate validity
            UpdateSigningCertificateIfNeeded();

            RefreshAlerts();
            _isLowChecking = false;
        }

        bool _isRefreshingAlerts = false;

        /// <summary>
        /// Refresh alerts hexagon badge icons visibility
        /// </summary>
        private void RefreshAlerts()
        {
            if (_isRefreshingAlerts)
                return;

            _isRefreshingAlerts = true;
            var reminders = _dbService.GetRemindersToDate(_sysUtility.Now).ToList();

            // StrutAdjustment alert visible if expired reminders has been found
            HasStrutAdjustmentAlert = reminders != null && reminders.Any(r => r.Type == Reminder.ReminderType.StrutAdjustmentReminder);
            // PinSiteCare alert visible if expired reminders has been found (only today alerts for pin site care)
            HasPinSiteCareAlert = reminders != null && reminders.Any(r => r.DateTime.Date == _sysUtility.Now.Date && r.Type == Reminder.ReminderType.PinSiteCareReminder);
            // Prescription alert visible if changes detected or app has pending sync
            HasPrescriptionAlert = _dbService.LastSyncHasChanges() || AppSettings.Instance.HasSyncPending;
            // Not UpToDate alert visible if last sync has been done more than 24 hours ago
            HasNotUpToDateAlert = IsLoggedIn && (!AppSettings.Instance.SyncLastDateTime.HasValue || AppSettings.Instance.SyncLastDateTime.Value < _sysUtility.Now.AddDays(-1));

            if (HasNotUpToDateAlert)
            {
                if (this.AppState != AppStateEnum.Outdated)
                    this.AppState = AppStateEnum.Outdated;
            }
            else if (AppSettings.Instance.HasSyncPending)
            {
                if (this.AppState != AppStateEnum.SyncPending)
                {
                    this.AppState = AppStateEnum.SyncPending;
                    OnPropertyChanged(nameof(IsStrutAdjustmentEnabled));
                }
            }
            else if (IsLoggedIn)
            {
                this.AppState = AppStateEnum.Synced;
                OnPropertyChanged(nameof(IsStrutAdjustmentEnabled));
            }
            else
            {
                this.AppState = AppStateEnum.Anonymous;
            }

            _isRefreshingAlerts = false;
        }

        private void CheckConnection()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                IsConnectionAvailable = true; // await _apiClient.IsServerReachable();
            }
            else
            {
                IsConnectionAvailable = false;
            }
        }

        private void UpdateLegalTermsLocalizedText()
        {
            try
            {
                //Get language then country
                var cultureName = Localization.LocalizationManager.CurrentCultureInfo.Name;
                var language = cultureName?.Split('-')?[0];
                if (language != null)
                {
                    //check for SIM country
                    var SIMcountry = Localization.LocalizationManager.GetSIMCountry()?.ToUpper();
                    if (!string.IsNullOrEmpty(SIMcountry) && SIMcountry.Equals("AU", StringComparison.CurrentCultureIgnoreCase))
                    {
                        cultureName = "en-AU";
                    }
                }
                string legalTermsFileName = string.Format("LegalTerms_{0}.htm", cultureName);

                LegalTermsAndPrivacyText = ResourceLoader.GetEmbeddedResourceString(typeof(HomeViewModel).GetTypeInfo().Assembly, legalTermsFileName);
            }
            catch
            {
                AppLoggerHelper.LogEvent("Localize legal terms", "Impossible to localize legal terms. The default (en-US) has been submitted", TraceLevel.Warning);
            }
            if (string.IsNullOrEmpty(LegalTermsAndPrivacyText))
            {
                LegalTermsAndPrivacyText = ResourceLoader.GetEmbeddedResourceString(typeof(HomeViewModel).GetTypeInfo().Assembly, "LegalTerms_en-US.htm");
            }
        }

        #region Local Notifications handlers

        /// <summary>
        /// Handler for local notifications
        /// </summary>
        /// <param name="notification"></param>
        public void OnLocalNotification(LocalNotification notification)
        {
            // Display an alert and navigate home if user can be disturbed
            if (!disableUI)
            {
                if (notification.NotificationType != LocalNotificationType.Generic)
                {
                    App.Current.MainPage.DisplayAlert(notification.Title, notification.Body, "OK");
                    App.NavigationController.NavigateToHome();
                    RefreshAlerts();
                }
            }
        }

        #endregion

        #region Remote Push notifications handlers

        /// <summary>
        /// Handler for received remote push notifications 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userInfo"></param>
        public void OnRemoteNotification(RemoteNotification remoteNotification)
        {
            switch (remoteNotification.NotificationType)
            {
                case RemoteNotificationType.WakeUp:
                    AppSettings.SetLastWakeUpDateTime(_sysUtility.Now);
                    RescheduleRemindersIfNeeded();
                    break;
                case RemoteNotificationType.Generic:
                    break;
                case RemoteNotificationType.MotivationalMessage:
                    if (IsLoggedIn && !disableUI)
                    {
                        AppLoggerHelper.LogEvent("Remote notification Received", "Motivational Message Receive from portal", TraceLevel.Verbose);

                        //Populate message view model.
                        App.ViewModelLocator.MotivationalMessage.MessageBody = remoteNotification.Body;
                        App.ViewModelLocator.MotivationalMessage.MessageCategory = remoteNotification.MessageCategory;
                        App.NavigationController.NavigateTo(NavigationController.MOTIVATIONAL_MESSAGE_POPUP_PAGE, true);

                    }
                    break;
                case RemoteNotificationType.Prescription:
                    AppLoggerHelper.LogEvent("Prescription Remote notification Received", "Forcing Sync prescription", TraceLevel.Verbose);

                    // mark Sync to be done 
                    AppSettings.SetHasSyncPending(true);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // run sync 
                        HiPriorityCheckCallback(null);
                    });

                    break;
                case RemoteNotificationType.DeviceChanged:
                    AppLoggerHelper.LogEvent("Device changed notification Received", "Forcing logout due a device changed notification", TraceLevel.Verbose);
                    if (!disableUI)
                    {
                        // Simulate login error (error code 1)
                        LoginFailedHandler(1);
                    }
                    break;

            }
        }

        /// <summary>
        /// Receive and store the registration token after push registration 
        /// </summary>
        /// <param name="Token"></param>
        public void OnRegistered(string Token)
        {
            Debug.WriteLine(string.Format("Push Notification - Device Registered - Token : {0}", Token));
            App.PushNotificationToken = Token;
        }

        /// <summary>
        /// Fires when device is unregistered
        /// </summary>
        public void OnUnregistered()
        {
            Debug.WriteLine("Push Notification - Device Unnregistered");
        }

        /// <summary>
        /// Fires on error
        /// </summary>
        /// <param name="message"></param>
        public void OnError(string message)
        {
            Debug.WriteLine(string.Format("Push notification error - {0}", message));
        }

        #endregion

        #region Sync / Update Methods

        private bool _isRefreshingCertificate = false;
        private object _RefreshingCertificateSyncObj = new object();

        /// <summary>
        /// Download a new signing certificate if needed
        /// </summary>
        private void UpdateSigningCertificateIfNeeded()
        {
            lock (_RefreshingCertificateSyncObj)
            {
                //if (_isSyncingSettings)
                //    return;

                if (AppSettings.Instance == null)
                    return;

                if (App.IsWizardUserSettingsActive)
                    return;

                // Check one time only
                if (App.IsCertificateChecked)
                    return;

                _isRefreshingCertificate = true;
            }

            UpdateSigningCertificate(_apiClient);

            lock (_RefreshingCertificateSyncObj)
            {
                _isRefreshingCertificate = false;
            }
        }

        private bool _isSyncingSettings = false;
        private object _SyncObjSettings = new object();

        /// <summary>
        /// Download user settings from portal server
        /// </summary>
        private async void SyncSettings()
        {
            lock (_SyncObjSettings)
            {
                if (!IsLoggedIn)
                    return;

                if (_isSyncingSettings)
                    return;

                if (AppSettings.Instance == null)
                    return;

                if (App.IsWizardUserSettingsActive)
                    return;

                _isSyncingSettings = true;
            }
            try
            {
                var settings = AppSettings.Instance;
                var loginResult = await _apiClient.Login(settings.GetApiUserName(), settings.GetApiPassword());
                IsBusy = false;
                if (loginResult.Success)
                {
                    var result = await _apiClient.GetSettings();
                    if (result.Success)
                    {
                        if (result.Data.PinSiteCareSettings != null)
                        {
                            // Save pin site care settings in something has changed                        
                            if (AppSettings.SetPortalSettingsIfChanged(result.Data.PinSiteCareSettings.Enabled, result.Data.PinSiteCareSettings.FirstDay, result.Data.PinSiteCareSettings.Frequency))
                            {
                                // Rebuild and reschedule pin site care reminders and notifications
                                RebuildPinSiteCareCalendarAndReminders(AppSettings.Instance.PinSiteCareTime);
                            }
                        }
                        else
                        {
                            AppLoggerHelper.LogEvent("ApiError", "Empty PinSiteSettings in GetSettings Api Call", TraceLevel.Warning);
                        }

                        if (result.Data.FinalTreatmentDateList != null)
                        {
                            // Prescriptions final treatment date provided: we have to update local saved prescriptions
                            if (!_dbService.UpdatePrescriptionsTreatmentDates(result.Data.FinalTreatmentDateList))
                            {
                                AppLoggerHelper.LogEvent("SyncSettings", "Error Updating Prescription Removal Date: " + _dbService.LastException?.ToString(), TraceLevel.Warning);
                            }
                        }

                        if (result.Data.SurgeonAddressInfo != null)
                        {
                            // Save surgeon contacts
                            var contacts = _dbService.GetSurgeonContacts();
                            if (contacts == null)
                                contacts = new SurgeonContacts();

                            contacts.FirstName = result.Data.SurgeonAddressInfo.FirstName;
                            contacts.LastName = result.Data.SurgeonAddressInfo.LastName;
                            contacts.AddressLine1 = result.Data.SurgeonAddressInfo.AddressLine1;
                            contacts.AddressLine2 = result.Data.SurgeonAddressInfo.AddressLine2;
                            contacts.City = result.Data.SurgeonAddressInfo.City;
                            contacts.StateProvince = result.Data.SurgeonAddressInfo.StateProvince;
                            contacts.PostalCode = result.Data.SurgeonAddressInfo.PostalCode;
                            contacts.Country = result.Data.SurgeonAddressInfo.Country != null ? result.Data.SurgeonAddressInfo.Country.Description : "";
                            contacts.Hospital = result.Data.SurgeonAddressInfo.Hospital;
                            contacts.OfficePhone = result.Data.SurgeonAddressInfo.OfficePhone;
                            contacts.MobilePhone = result.Data.SurgeonAddressInfo.MobilePhone;

                            if (_dbService.SaveSurgeonContacts(contacts))
                            {
                                MessagingCenter.Send<BaseViewModel>(this, Messaging.Messages.SURGEON_CONTACTS_UPDATED);
                            }
                            else
                            {
                                AppLoggerHelper.LogEvent("LocalDatabase", "Failed to save surgeon contacts" + _dbService.LastException?.ToString(), TraceLevel.Warning);
                            }
                        }

                    }
                    else
                    {
                        AppLoggerHelper.LogEvent("ApiError", "GetSettings failed:" + result.ErrorMessage, TraceLevel.Error);
                    }
                }
                else
                {
                    LoginFailedHandler(loginResult.ErrorCode);
                }
            }
            catch (Exception ex)
            {
                AppLoggerHelper.LogEvent("Unhandled SyncSettings exception", ex != null ? ex.ToString() : "No details", Newtonsoft.Json.TraceLevel.Error);
            }
            finally
            {
                lock (_SyncObjSettings)
                {
                    _isSyncingSettings = false;
                }
            }
        }

        /// <summary>
        /// Upload user settings if something is changed since last upload
        /// </summary>
        private async void UploadSettingsIfNeeded()
        {
            if (!IsLoggedIn)
                return;

            var settings = AppSettings.Instance;
            var loginResult = await _apiClient.Login(settings.GetApiUserName(), settings.GetApiPassword());
            IsBusy = false;
            if (loginResult.Success)
            {
                var historyLogItems = _dbService.GetUnsyncedHistoryLogItems();
                var eventItems = new List<PatientDiaryEvent>();

                foreach (var item in historyLogItems)
                {
                    var eventItem = new PatientDiaryEvent()
                    {
                        Description = item.Description,
                        EntityExpectedDate = item.ExpectedDateTime,
                        EventDate = item.EventDateTime,
                        EntityId = item.ServerEntityId,
                        Id = item.Id
                    };

                    // Retrieve Case unique identifier for strut adjustments events
                    if (item.ItemType == LogHistoryItem.ItemTypeEnum.StrutAdjustmentDone || item.ItemType == LogHistoryItem.ItemTypeEnum.StrutAdjustmentPostponed)
                    {
                        var strut = _dbService.GetStrutAdjustmentById(item.LocalEntityId.Value);
                        if (strut != null)
                        {
                            var prescription = _dbService.GetPrescritionById(strut.PrescriptionId);
                            if (prescription != null)
                            {
                                eventItem.CaseUid = prescription.CaseId;
                            }
                        }
                    }
                    switch (item.ItemType)
                    {
                        case LogHistoryItem.ItemTypeEnum.PersonalGoalDisabledEnabled:
                            eventItem.Type = ((int)PatientLogHistoryType.PersonalGoalSet).ToString();
                            break;
                        case LogHistoryItem.ItemTypeEnum.PinSiteCareDone:
                            eventItem.Type = ((int)PatientLogHistoryType.PinSiteCareDone).ToString();
                            break;
                        case LogHistoryItem.ItemTypeEnum.StrutAdjustmentDone:
                            eventItem.Type = ((int)PatientLogHistoryType.StrutAdjustmentDone).ToString();
                            break;
                        case LogHistoryItem.ItemTypeEnum.StrutAdjustmentPostponed:
                            eventItem.Type = ((int)PatientLogHistoryType.StrutAdjustmentPostponed).ToString();
                            break;
                        case LogHistoryItem.ItemTypeEnum.MotivationalMesssageRead:
                            eventItem.Type = ((int)PatientLogHistoryType.MotivationalMessageRead).ToString();
                            break;
                        case LogHistoryItem.ItemTypeEnum.MoodSelfAssessment:
                            eventItem.Type = ((int)PatientLogHistoryType.DailyMoodSurveyAnswered).ToString();
                            break;
                        case LogHistoryItem.ItemTypeEnum.AppStateChanged:
                            eventItem.Type = ((int)PatientLogHistoryType.AppStateChanged).ToString();
                            break;
                        case LogHistoryItem.ItemTypeEnum.PrescriptionUpdated:
                            eventItem.Type = ((int)PatientLogHistoryType.PrescriptionUpdated).ToString();
                            break;
                        case LogHistoryItem.ItemTypeEnum.MotivationalMesssageDisabledEnabled:
                            eventItem.Type = ((int)PatientLogHistoryType.MotivationalMessageFlag).ToString();
                            break;
                    }

                    eventItems.Add(eventItem);
                }

                bool result = false;
                try
                {
                    var sendResult = await _apiClient.UploadSettings(eventItems, settings.PinSiteCareTime, settings.IsGoalEnabled, settings.IsInsightEnabled, settings.InsightTime, true, App.RuntimVersion);
                    result = sendResult.Success;
                }
                catch (Exception ex)
                {
                    AppLoggerHelper.LogException(ex, "Failed to UploadSettings", TraceLevel.Error);
                }

                if (result)
                {
                    if (_dbService.MarkAsSyncedHistoryLogItems(historyLogItems))
                    {
                        // Save upload datetime
                        AppSettings.SetSettingsLastUploadDateTime(_sysUtility.Now);
                    }
                    else
                    {
                        AppLoggerHelper.LogEvent("MarkAsSyncedHistoryLogItems", "Failed to mark as synced " + _dbService.LastException ?? "(UNKNOWN EXCEPTION)", TraceLevel.Error);
                    }

                }
            }
            else
            {
                LoginFailedHandler(loginResult.ErrorCode);
            }
        }

        private bool _isSyncing = false;
        private object _SyncObjPrescr = new object();

        /// <summary>
        /// Download prescriptions updates from portal server
        /// </summary>
        private async void SyncPrescriptions()
        {
            lock (_SyncObjPrescr)
            {
                if (!IsLoggedIn)
                    return;

                if (_isSyncing)
                    return;

                if (AppSettings.Instance == null)
                    return;

                if (!App.IsCertificateChecked)
                    return;

                _isSyncing = true;
            }
            try
            {
                var settings = AppSettings.Instance;
                var loginResult = await _apiClient.Login(settings.GetApiUserName(), settings.GetApiPassword());
                if (loginResult.Success)
                {
                    var updateResult = await _apiClient.DownloadUpdatePackage();
                    if (updateResult.Success)
                    {
                        // Check sign
                        if (VerifySign(updateResult.Data.Updates, updateResult.Data.Sign))
                        {
                            if (updateResult.Data.Updates.HasChanges)
                            {
                                if (_dbService.SaveDownloadedPrescriptionsUpdate(updateResult.Data.Updates))
                                {
                                    var syncCompletedResult = await _apiClient.SetSyncCompleted();
                                    if (syncCompletedResult.Success)
                                    {
                                        // Reset pending sync
                                        AppSettings.SetHasSyncPending(false);

                                        // set a flag if the prescription has struts older then today
                                        // it will be used in strut adjustment recap for asking user to skip or not                                                            
                                        if (_dbService.ExistStrutAdjustmentsToDate(_sysUtility.Now.Date.AddDays(-1)))
                                        {
                                            AppSettings.SetShowStrutsSkippedWarning(true);
                                        }

                                        // Force rescheduling if needed
                                        AppSettings.SetReminderLastUpdate(_sysUtility.Now);

                                        // Send local notification
                                        var notification = new LocalNotification()
                                        {
                                            DateTime = _sysUtility.Now,
                                            NotificationType = LocalNotificationType.PrescriptionUpdated,
                                            Title = Resources.PatientApp.NotificationPrescriptionUpdateTitle,
                                            //Body = sb.ToString()
                                            Body = Resources.PatientApp.NotificationPrescriptionUpdateBody
                                        };
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            DependencyService.Get<INotificationManager>().ScheduleLocalNotification(notification);
                                        });

                                        // Save in log history
                                        var historyItem = new LogHistoryItem()
                                        {
                                            LocalEntityId = null,
                                            ExpectedDateTime = null,
                                            EventDateTime = _sysUtility.Now,
                                            ItemType = LogHistoryItem.ItemTypeEnum.PrescriptionUpdated,
                                            Description = Resources.PatientApp.NotificationPrescriptionUpdateTitle,
                                            ServerEntityId = null,
                                        };
                                        _dbService.SaveHistoryLogItem(historyItem);

                                    }
                                    else
                                    {
                                        AppLoggerHelper.LogEvent("SyncError", "Error sending sync completed: " + syncCompletedResult.ErrorMessage ?? "(unknown)", TraceLevel.Error);
                                    }
                                }
                                else
                                {
                                    AppLoggerHelper.LogEvent("Prescription Save", "Error saving downloaded prescriptiont to local database", TraceLevel.Error);
                                }
                            }
                            else
                            {
                                var syncCompletedResult = await _apiClient.SetSyncCompleted();
                                if (syncCompletedResult.Success)
                                {
                                    // Reset pending sync
                                    if (AppSettings.Instance.HasSyncPending)
                                        AppSettings.SetHasSyncPending(false);
                                }
                            }
                            AppSettings.SetSyncLastDateTime(_sysUtility.Now);

                        }
                        else
                        {
                            AppLoggerHelper.LogEvent("SignError", "Downloaded sync update has wrong sign", TraceLevel.Error);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(updateResult.ErrorMessage))
                        {
                            AppLoggerHelper.LogEvent("DownloadUpdatePackage", string.Format("Error Code: {0} - {1}", updateResult.ErrorCode, updateResult.ErrorMessage), TraceLevel.Error);
                        }
                        else
                        {
                            AppLoggerHelper.LogEvent("DownloadUpdatePackage", string.Format("Error Code: {0}", updateResult.ErrorCode), TraceLevel.Error);
                        }
                        LoginFailedHandler(updateResult.ErrorCode);
                    }

                }
                else
                {
                    LoginFailedHandler(loginResult.ErrorCode);
                }
            }
            catch (Exception ex)
            {
                AppLoggerHelper.LogEvent("Unhandled SyncPrescription exception", ex != null ? ex.ToString() : "No details", Newtonsoft.Json.TraceLevel.Error);
            }
            finally
            {
                lock (_SyncObjPrescr)
                {
                    _isSyncing = false;
                }
            }
        }
        #endregion

        /// <summary>
        /// Manage the application behavior on login failed
        /// </summary>
        /// <param name="errorCode"></param>
        private void LoginFailedHandler(int errorCode)
        {
            if (errorCode == 1 && IsLoggedIn)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage.DisplayAlert(Resources.PatientApp.ErrorTitle, Resources.PatientApp.ErrorLoggedOut, Resources.PatientApp.BtnOK);
                    App.NavigationController.NavigateToHome();
                    AppSettings.ResetToAnonymous();
                    NotifyLoginState(false);
                    if (_dbService.ResetToAnonymous())
                    {
                        var service = DependencyService.Get<INotificationManager>();
                        service.DeleteAllLocalNotifications();
                    }
                    else
                    {
                        AppLoggerHelper.LogEvent("ResetToAnonymous", string.Format("Error : {0}", _dbService.LastException.Message), TraceLevel.Error);
                    }
                });
            }
            else
            {
                // Do nothing
            }

        }

    }
}