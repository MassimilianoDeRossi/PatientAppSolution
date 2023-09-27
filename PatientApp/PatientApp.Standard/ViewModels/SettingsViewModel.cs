using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

using MyHexPlanProxies.Models;
using PatientApp.Interfaces;
using PatientApp.Views;
using PatientApp.Services;
using PatientApp.Settings;
using PatientApp.DataModel.SqlEntities;
using PatientApp.Localization;

namespace PatientApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private TapGestureRecognizerCustom _easterEggManager = null;

        public Command TestModeCommand { get; set; }
        public Command<string> OpenLinkCommand { get; set; }
        public Command SendFeedbackCommand { get; set; }
        public Command SetLanguageCommand { get; set; }

        public bool IsTestMode
        {
            get { return App.TestModel.TestModeOn; }
        }

        /// <summary>
        /// Settings
        /// </summary>
        private bool _isInsightEnabled;
        public bool IsInsightEnabled
        {
            get { return _isInsightEnabled; }
            set { SetProperty(ref _isInsightEnabled, value); }
        }

        private TimeSpan _insightTime;
        public TimeSpan InsightTime
        {
            get { return _insightTime; }
            set { SetProperty(ref _insightTime, value); }
        }

        private bool _isGoalEnabled;
        public bool IsGoalEnabled
        {
            get { return _isGoalEnabled; }
            set { SetProperty(ref _isGoalEnabled, value); }
        }

        private string _personalGoal;
        public string PersonalGoal
        {
            get { return _personalGoal; }
            set { SetProperty(ref _personalGoal, value); }
        }

        private TimeSpan _pinSiteCareTime;
        public TimeSpan PinSiteCareTime
        {
            get { return _pinSiteCareTime; }
            set { SetProperty(ref _pinSiteCareTime, value); }
        }

        string _applicationInstanceId = string.Empty;
        public string ApplicationInstanceId
        {
            get { return _applicationInstanceId; }
            set { SetProperty(ref _applicationInstanceId, value); }
        }

        string _signCertificate = string.Empty;
        public string SignCertificate
        {
            get { return _signCertificate; }
            set { SetProperty(ref _signCertificate, value); }
        }

        string _bundleVersion = string.Empty;
        public string BundleVersion
        {
            get { return _bundleVersion; }
            set { SetProperty(ref _bundleVersion, value); }
        }

        private bool _isLanguageSelectionEnabled;
        public bool IsLanguageSelectionEnabled
        {
            get { return _isLanguageSelectionEnabled; }
            set { SetProperty(ref _isLanguageSelectionEnabled, value); }
        }

        List<LanguageItem> _languages = null;
        public List<LanguageItem> Languages
        {
            get { return _languages; }
            set { SetProperty(ref _languages, value); }
        }

        LanguageItem _selectedLanguage = null;
        public LanguageItem SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { SetProperty(ref _selectedLanguage, value); }
        }

        public SettingsViewModel(ILocalDatabaseService dbService, IApiClient apiClient, ISystemUtility sysUtility) : base(dbService, apiClient, sysUtility)
        {
            _easterEggManager = new TapGestureRecognizerCustom(5, 300);

            SignCertificate = PCLAppConfig.ConfigurationManager.AppSettings["SignCertificate"];
            BundleVersion = App.RuntimVersion;//DependencyService.Get<IVersionService>().GetBundleVersion();

            TestModeCommand = new Command(TestModeCommandExecute);
            ApplicationInstanceId = Settings.AppSettings.Instance.ApplicationInstanceId.ToString();
            OpenLinkCommand = new Command<string>(OpenLinkCommandExecute);
            SendFeedbackCommand = new Command(SendFeedbackCommandExecute);
            SetLanguageCommand = new Command(SetLanguageCommandExecute);

            Languages = LocalizationManager.AvailableLanguages;

            IsLanguageSelectionEnabled = PCLAppConfig.ConfigurationManager.AppSettings["IsLanguageSelectionEnabled"] == "1";

            MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, (page) =>
                {
                    // Refresh data on view appearing
                    if (page is PinSiteCareTimePage)
                    {
                        this.PinSiteCareTime = AppSettings.Instance.PinSiteCareTime;
                    }
                    if (page is InsightMessagePage)
                    {
                        this.IsInsightEnabled = AppSettings.Instance.IsInsightEnabled;
                        this.InsightTime = AppSettings.Instance.InsightTime;
                    }
                    if (page is PersonalGoalPage)
                    {
                        this.IsGoalEnabled = AppSettings.Instance.IsGoalEnabled;
                        this.PersonalGoal = AppSettings.Instance.PersonalGoal;
                    }
                    if (page is LanguagePage)
                    {
                        RefreshLanguageSelection();
                    }
                });

            MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_DISAPPEARING_MESSAGE, (page) =>
                {
                    if (page is PinSiteCareTimePage)
                    {
                        // Save to app settings
                        if (this.PinSiteCareTime != AppSettings.Instance.PinSiteCareTime)
                        {
                            RebuildPinSiteCareCalendarAndReminders(this.PinSiteCareTime);
                        }
                    }
                    if (page is InsightMessagePage)
                    {
                        if (AppSettings.Instance.IsInsightEnabled != this.IsInsightEnabled)
                        {
                            // Save in log history
                            var historyItem = new LogHistoryItem()
                            {
                                LocalEntityId = null,
                                ExpectedDateTime = null,
                                EventDateTime = _sysUtility.Now,
                                ItemType = LogHistoryItem.ItemTypeEnum.MotivationalMesssageDisabledEnabled,
                                Description = this.IsInsightEnabled ? "Motivational messages ENABLED" : "Motivational messages DISABLED",
                                ServerEntityId = null,
                            };
                            _dbService.SaveHistoryLogItem(historyItem);
                        }

                        // Save to app settings
                        AppSettings.SetInsight(this.IsInsightEnabled, this.InsightTime);
                    }
                    if (page is PersonalGoalPage)
                    {
                        if (AppSettings.Instance.IsGoalEnabled != this.IsGoalEnabled)
                        {
                            // Save in log history
                            var historyItem = new LogHistoryItem()
                            {
                                LocalEntityId = null,
                                ExpectedDateTime = null,
                                EventDateTime = _sysUtility.Now,
                                ItemType = LogHistoryItem.ItemTypeEnum.PersonalGoalDisabledEnabled,
                                Description = this.IsGoalEnabled ? "Personal Goal ENABLED" : "Personal Goal DISABLED",
                                ServerEntityId = null,
                            };
                            _dbService.SaveHistoryLogItem(historyItem);
                        }
                        // Save to app settings
                        AppSettings.SetPersonalGoal(this.IsGoalEnabled, this.PersonalGoal);

                    }
                });
        }

        private void TestModeCommandExecute()
        {
            if (_easterEggManager.IsWishedTap())
            {
                App.TestModel.TestModeOn = !App.TestModel.TestModeOn;
            }
            OnPropertyChanged(nameof(IsTestMode));
        }

        private void OpenLinkCommandExecute(string link)
        {
            Device.OpenUri(new Uri("http://" + link));
        }

        private void SendFeedbackCommandExecute()
        {
            DependencyService.Get<IFeedbackService>().GetFeedback();
        }

        private void SetLanguageCommandExecute()
        {
            if (SelectedLanguage != null)
            {
                LocalizationManager.SetCurrentLanguage(SelectedLanguage.Code);
                AppSettings.SetLanguageCode(SelectedLanguage.Code);
                RefreshLanguageSelection();
                App.NavigationController.NavigateBack();
            }
        }

        private void RefreshLanguageSelection()
        {
            var currentLang = LocalizationManager.GetCurrentLanguageCode();
            foreach (var lang in Languages)
            {
                if (lang.Code.Equals(currentLang, StringComparison.CurrentCultureIgnoreCase))
                {
                    SelectedLanguage = lang;
                    SelectedLanguage.IsSelected = true;
                }
                else
                    lang.IsSelected = false;
            }
            OnPropertyChanged(nameof(Languages));
        }
    }

    /// <summary>
    /// Check if user tapped N times in a X interval. Every time the function IsWishedTap is called, it registers a tap and return if is the desired one.
    /// On Android 'TapGestureRecognizer' cannot register more then 2 clicks. To fix this problem we implemented a custom TapGestureRecognizer.
    /// </summary>
    public class TapGestureRecognizerCustom
    {
        private int _numberOfTapsRequired;
        private int _toleranceInMs;

        private DateTime? _lastTap = null;
        private int _numberOfTaps = 0;

        /// <summary>
        /// Initialize parameters
        /// </summary>
        /// <param name="numberOfTapsRequired">Number of taps required</param>
        /// <param name="toleranceInMs">Max waiting time between taps</param>
        public TapGestureRecognizerCustom(int numberOfTapsRequired = 5, int toleranceInMs = 300)
        {
            _toleranceInMs = toleranceInMs;
            _numberOfTapsRequired = numberOfTapsRequired;
        }

        private void SetNumberOfTapsAndLastTap(int numberOfTaps, DateTime? lastTap)
        {
            _numberOfTaps = numberOfTaps;
            _lastTap = lastTap;
        }

        /// <summary>
        /// Register tap and checks if it's the desired one
        /// </summary>
        public bool IsWishedTap()
        {
            if (_lastTap == null || (DateTime.Now - _lastTap.Value).TotalMilliseconds < _toleranceInMs)
            {
                if (_numberOfTaps == _numberOfTapsRequired - 1)
                {
                    SetNumberOfTapsAndLastTap(0, null);
                    return true;
                }
                else
                {
                    SetNumberOfTapsAndLastTap(_numberOfTaps + 1, DateTime.Now);
                }
            }
            else
            {
                SetNumberOfTapsAndLastTap(1, DateTime.Now);
            }
            return false;
        }
    }
}