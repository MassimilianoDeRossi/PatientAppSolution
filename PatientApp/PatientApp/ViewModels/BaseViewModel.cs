using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using Xamarin.Forms;

using PatientApp.Utilities;
using PatientApp.Views;
using PatientApp.Settings;
using PatientApp.Services;
using PatientApp.Interfaces;
using PatientApp.DataModel.SqlEntities;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// Base abstract viewmodel containing common behaviors
    /// </summary>
    public abstract class BaseViewModel : ObservableObject
    {
        private bool _isRescheduling = false;
        private int _rescheduleRangeDays = 1;

        protected ILocalDatabaseService _dbService = null;
        protected IApiClient _apiClient;
        protected ISystemUtility _sysUtility = null;

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        /// <summary>
        /// Property indicating the user login state
        /// </summary>
        public bool IsLoggedIn
        {
            get { return AppSettings.Instance.IsLoggedIn; }
        }

        /// <summary>
        /// Command used for main navigation. The parameter is the navigation target 
        /// </summary>
        public Command<string> MenuCommand { get; set; }

        /// <summary>
        /// Command used to navigate back
        /// </summary>
        public Command GoBackCommand { get; set; }

        protected BaseViewModel(ILocalDatabaseService dbService, IApiClient apiClient, ISystemUtility sysUtility)
        {
            _dbService = dbService;
            _apiClient = apiClient;
            _sysUtility = sysUtility;

            var configRangeDays = PCLAppConfig.ConfigurationManager.AppSettings["ReschedulingRangeDays"];
            if (!string.IsNullOrEmpty(configRangeDays))
            {
                _rescheduleRangeDays = int.Parse(configRangeDays);
            }

            MessagingCenter.Subscribe<BaseViewModel>(this, Messaging.Messages.USER_LOGGED_IN, (sender) =>
            {
                OnPropertyChanged(nameof(IsLoggedIn));
            });
            MessagingCenter.Subscribe<BaseViewModel>(this, Messaging.Messages.USER_LOGGED_OUT, (sender) =>
            {
                OnPropertyChanged(nameof(IsLoggedIn));
            });

            MenuCommand = new Command<string>(MenuCommandExecute);
            GoBackCommand = new Command(GoBackCommandExecute);
        }

        private void MenuCommandExecute(string target)
        {
            App.NavigationController.NavigateTo(target);
        }

        private void GoBackCommandExecute()
        {
            DependencyService.Get<IOrientationManager>().ForcePortrait();
            App.NavigationController.NavigateBack();
        }

        /// <summary>
        /// Show an alert with an error message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected void ShowErrorMessage(string title, string message)
        {
            var popupPage = new ErrorMessagePopup(title, message);
            App.NavigationController.NavigateToPage(popupPage, true, true);
        }

        /// <summary>
        /// Show an alert with an info message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected void ShowInfoMessage(string title, string message)
        {
            var popupPage = new InfoMessagePopup(title, message);
            App.NavigationController.NavigateToPage(popupPage, true, true);
        }

        /// <summary>
        /// Send a broadcast message to notificate user login state updates
        /// </summary>
        /// <param name="loggedIn"></param>
        protected void NotifyLoginState(bool loggedIn)
        {
            if (loggedIn)
            {
                MessagingCenter.Send<BaseViewModel>(this, Messaging.Messages.USER_LOGGED_IN);
            }
            else
            {
                MessagingCenter.Send<BaseViewModel>(this, Messaging.Messages.USER_LOGGED_OUT);
            }
        }

        protected void RebuildPinSiteCareCalendarAndReminders(TimeSpan pinSiteCareTime)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                IsBusy = true;
                AppSettings.SetPinSiteCare(true, pinSiteCareTime, _sysUtility.Now);
                // force rebuild of pin site care calendar
                _dbService.RebuildPinSiteCareCalendar(IsLoggedIn, AppSettings.Instance.PinSiteCareEnabled, AppSettings.Instance.PinSiteCareTime,
                                                      AppSettings.Instance.PinSiteCareStartDate, AppSettings.Instance.PinSiteCareDaysOfWeekNotification);
                // Force rescheduling 
                AppSettings.SetReminderLastUpdate(_sysUtility.Now);
                RescheduleRemindersIfNeeded();
                IsBusy = false;
            });
        }

        /// <summary>
        /// Perform reminder rescheduling if something is changed in the database since last scheduling
        /// </summary>
        protected void RescheduleRemindersIfNeeded()
        {
            if (_isRescheduling)
                return;

            _isRescheduling = true;
            var settings = AppSettings.Instance;
            // Need a new scheduling if we have reminders and:
            // we have never scheduled 
            // or we have scheduled but reminders have been updated after last rescheduling
            // or we have scheduled but more than a day ago
            if (settings.ReminderLastUpdate.HasValue &&
                (!settings.SchedulingLastDateTime.HasValue ||
                  settings.SchedulingLastDateTime.Value < settings.ReminderLastUpdate.Value ||
                  settings.SchedulingLastDateTime.Value.Date < _sysUtility.Now.Date))
            {
                AppLoggerHelper.LogEvent("Local notifications", "Rescheduling Local notifications", TraceLevel.Info);
                var service = DependencyService.Get<INotificationManager>();
                bool deleted = false;
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        service.DeleteAllLocalNotifications();
                    });
                    deleted = true;
                }
                catch (Exception ex)
                {
                    AppLoggerHelper.LogException(ex, "Failed deletion of all local notifications", TraceLevel.Error);
                }
                if (deleted)
                {
                    // Get list of reminders to schedule (from now to today+_rescheduleRangeDays days at midnight)
                    // _rescheduleRangeDays is taken from app.config
                    var endDateTime = _sysUtility.Now.Date.AddDays(_rescheduleRangeDays + 1).AddSeconds(-1);

                    var reminders = _dbService.GetReminders().Where(r => !r.IsChecked && r.DateTime > _sysUtility.Now && r.DateTime <= endDateTime).OrderBy(r => r.DateTime);
                    foreach (var reminder in reminders)
                    {
                        var notification = new LocalNotification()
                        {
                            DateTime = reminder.DateTime,
                            PrescriptionId = reminder.PrescriptionId,
                            EntityId = reminder.EntityId,
                        };

                        switch (reminder.Type)
                        {
                            case DataModel.SqlEntities.Reminder.ReminderType.StrutAdjustmentReminder:
                                notification.NotificationType = LocalNotificationType.StrutAdjustmentReminder;
                                notification.Title = string.Concat(Localization.LocalizationManager.GetText("NotificationStrutAdjustmentTitle"), " ", reminder.DateTime.ToString("h tt"));
                                notification.Body = Localization.LocalizationManager.GetText("NotificationStrutAdjustmentBody");
                                break;
                            case DataModel.SqlEntities.Reminder.ReminderType.PinSiteCareReminder:
                                notification.NotificationType = LocalNotificationType.PinSiteCareReminder;
                                notification.Title = string.Concat(Localization.LocalizationManager.GetText("NotificationPinSiteCareTitle"), " ", reminder.DateTime.ToString("h:mm tt"));
                                notification.Body = Localization.LocalizationManager.GetText("NotificationPinSiteCareBody");
                                break;
                            default:
                                notification.NotificationType = LocalNotificationType.Generic;
                                notification.Title = reminder.Type.ToString();
                                notification.Body = notification.Title;
                                break;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            service.ScheduleLocalNotification(notification);
                        });
                    }
                    // Save scheduling datetime
                    AppSettings.SetSchedulingLastDateTime(_sysUtility.Now);
                }
            }

            _isRescheduling = false;
        }

        protected async Task UpdateSigningCertificate(IApiClient _apiClient)
        {
            try
            {
                var settings = AppSettings.Instance;
                var result = await _apiClient.GetSigningCertificate(settings.LastSigningCertificateUpdate);
                IsBusy = false;
                if (result.Success)
                {
                    if (result.Data.Exists)
                    {
                        AppSettings.SetSigningCertificate(result.Data.Certificate, DateTime.Today);
                    }
                    App.IsCertificateChecked = AppSettings.Instance.SigningCertificateContent != null;
                }
                else
                {
                    AppLoggerHelper.LogEvent("ApiError", "GetSigningCertificate failed:" + result.ErrorMessage, TraceLevel.Error);
                }
            }
            catch (Exception ex)
            {
                AppLoggerHelper.LogEvent("Unhandled GetSigningCertificate exception", ex != null ? ex.ToString() : "No details", Newtonsoft.Json.TraceLevel.Error);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Verify crypted object signature
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        protected bool VerifySign(object data, string sign)
        {
            var certContent = AppSettings.Instance.SigningCertificateContent;
            var check = DependencyService.Get<ICryptoService>().VerifySign(data, sign, certContent);
            return check || System.Diagnostics.Debugger.IsAttached;
        }


    }
}

