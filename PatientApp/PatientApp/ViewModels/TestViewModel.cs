using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using PatientApp.DataModel.SqlEntities;
using PatientApp.Interfaces;
using PatientApp.Settings;
using System.Collections.ObjectModel;
using PatientApp.Views;
using PatientApp.Services;
using System.Threading;
using Microsoft.AppCenter.Crashes;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// A view model used for debug and test purposes
    /// </summary>
    public class TestViewModel : BaseViewModel
    {
        public Command PrescriptionUpdateNotificationCommand { get; set; }
        public Command MotivationalMessageNotificationCommand { get; set; }
        public Command LocalNotificationCommand { get; set; }
        public Command SetOutOfDateCommand { get; set; }
        public Command TestCommand { get; set; }
        public Command ResetAllCommand { get; set; }
        public Command StressTestCommand { get; set; }
        public Command SimulateCrashCommand { get; set; }

        public string ApplicationId { get; set; }
        public string PushToken { get; set; }
        public string BuildConfiguration { get; set; }
        public string ApiUrl { get; set; }
        public bool EnabledDebugTools { get; set; }

        public int ScheduledLocalNotificationsCount { get; set; }

        public ObservableCollection<Reminder> Reminders { get; protected set; }
        public ObservableCollection<LocalNotification> ScheduledNotifications { get; protected set; }

        public TestViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
        {
            PrescriptionUpdateNotificationCommand = new Command(PrescriptionUpdateNotificationCommandExecute);
            MotivationalMessageNotificationCommand = new Command(MotivationalMessageNotificationCommandExecute);
            LocalNotificationCommand = new Command(LocalNotificationCommandExecute);
            SetOutOfDateCommand = new Command(SetOutOfDateCommandExecute);
            TestCommand = new Command(TestCommandExecute);
            ResetAllCommand = new Command(ResetAllCommandExecute);
            StressTestCommand = new Command(StressTestCommandExecute3);
            SimulateCrashCommand = new Command(SimulateCrashCommandExecute);

            ApplicationId = AppSettings.Instance.ApplicationInstanceId.ToString();
            PushToken = App.PushNotificationToken;

            BuildConfiguration = "UNKNOWN";

#if ENABLE_TEST_CLOUD && DEBUG
            BuildConfiguration = "DEBUG TEST CLOUD";
#endif
#if ENABLE_TEST_CLOUD && !DEBUG
            BuildConfiguration = "TEST CLOUD";
#endif
#if !ENABLE_TEST_CLOUD && DEBUG
            BuildConfiguration = "DEBUG";
#endif
#if !ENABLE_TEST_CLOUD && !DEBUG
            BuildConfiguration = "RELEASE";
#endif

            ApiUrl = PCLAppConfig.ConfigurationManager.AppSettings["ApiUrl"];

            OnPropertyChanged(nameof(ApplicationId));
            OnPropertyChanged(nameof(PushToken));
            OnPropertyChanged(nameof(BuildConfiguration));

            Reminders = new ObservableCollection<Reminder>();
            ScheduledNotifications = new ObservableCollection<LocalNotification>();

            MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, (page) =>
            {
                if (page is TestPage)
                {
                    EnabledDebugTools = System.Diagnostics.Debugger.IsAttached;
                    OnPropertyChanged(nameof(EnabledDebugTools));

                    Reminders = new ObservableCollection<Reminder>(_dbService.GetRemindersAtDate(_sysUtility.Now).ToList());
                    OnPropertyChanged(nameof(Reminders));

                    LoadScheduledNotifications();
                }
            });
        }

        private void LoadScheduledNotifications()
        {
            var service = DependencyService.Get<INotificationManager>();
            ScheduledNotifications = new ObservableCollection<LocalNotification>(service.GetScheduledLocalNotifications());
            ScheduledLocalNotificationsCount = ScheduledNotifications.Count;
            OnPropertyChanged(nameof(ScheduledNotifications));
            OnPropertyChanged(nameof(ScheduledLocalNotificationsCount));
        }

        const int loops = 10000;
        private void StressTestCommandExecute()
        {
            Task.Factory.StartNew(
                (objectState) =>
                {
                    System.Diagnostics.Debug.WriteLine("CREATING FAKE REMINDERS...");
                    for (int i = 1; i <= loops; i++)
                    {
                        var result = _dbService.SaveReminder(new Reminder()
                        {
                            DateTime = DateTime.Now,
                            Type = Reminder.ReminderType.PinSiteCareReminder,
                            IsChecked = true // skip notification                             
                        });
                    }

                    System.Diagnostics.Debug.WriteLine("FAKE REMINDERS CREATION COMPLETED");


                    int success = 0;
                    int fail = 0;
                    System.Diagnostics.Debug.WriteLine("CREATING FAKE STRUTS ITEMS...");
                    for (int i = 1; i <= loops; i++)
                    {
                        var result = _dbService.SaveStrutAdjustment(new StrutAdjustment()
                        {
                            DateOfAdjustment = DateTime.Now,
                            Done = false,
                        });

                        if (result)
                            success++;
                        else
                        {
                            fail++;
                            System.Diagnostics.Debug.WriteLine("FAIL: " + _dbService.LastException.Message);
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("CREATING FAKE STRUTS completed: SUCCEDED: {0} - FAILED: {1} ", success, fail);

                }
                , TaskCreationOptions.LongRunning
                , new CancellationTokenSource().Token);

            Task.Factory.StartNew(
                (objectState) =>
                {
                    _dbService.DeleteStrutAdjustmentsToDate(DateTime.Now, true);
                }
                , TaskCreationOptions.LongRunning
                , new CancellationTokenSource().Token);

            Task.Factory.StartNew(
                (objectState) =>
                {
                    int success = 0;
                    int fail = 0;
                    var reminders = _dbService.GetReminders().ToList();
                    System.Diagnostics.Debug.WriteLine("DELETING ALL REMINDERS...");
                    foreach (var r in reminders)
                    {
                        var result = _dbService.DeleteReminder(r);
                        if (result)
                            success++;
                        else
                        {
                            fail++;
                            System.Diagnostics.Debug.WriteLine("FAIL: " + _dbService.LastException.Message);
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("DELETING REMINDERS completed: SUCCEDED: {0} - FAILED: {1} ", success, fail);

                }
                , TaskCreationOptions.LongRunning
                , new CancellationTokenSource().Token);

        }

        private void StressTestCommandExecute2()
        {
            var dt = _sysUtility.Now;
            var prescription = _dbService.GetPrescriptions().First();

            for (int i = 1; i <= 1000; i++)
            {
                dt = dt.AddMinutes(-5);
                var strut = new StrutAdjustment()
                {
                    DateOfAdjustment = dt,
                    PrescriptionId = prescription.Id,
                    Done = false,
                    TreatmentStepNumber = 1
                };
                _dbService.SaveStrutAdjustment(strut);
                var reminder = new Reminder()
                {
                    DateTime = dt,
                    Type = Reminder.ReminderType.StrutAdjustmentReminder,
                    IsChecked = false,
                    EntityId = strut.Id,
                    PrescriptionId = prescription.Id
                };
                _dbService.SaveReminder(reminder);
            }
            AppSettings.SetShowStrutsSkippedWarning(true);
        }

        private void StressTestCommandExecute3()
        {
            try
            {
                var view = new WizardUserSettingsPage();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void SetOutOfDateCommandExecute()
        {
            AppSettings.SetSyncLastDateTime(_sysUtility.Now.AddDays(-2));
        }

        private void PrescriptionUpdateNotificationCommandExecute()
        {
            var remoteNotification = new RemoteNotification()
            {
                Id = "1",
                NotificationType = RemoteNotificationType.Prescription,
                Body = "Test Prescription Update notification",
            };

            App.ViewModelLocator.Home.OnRemoteNotification(remoteNotification);
        }

        private async void MotivationalMessageNotificationCommandExecute()
        {
            App.Current.MainPage.DisplayAlert("Test", "This is a test alert", "OK");

            await Task.Delay(2000);

            var remoteNotification = new RemoteNotification()
            {
                Id = "1",
                NotificationType = RemoteNotificationType.MotivationalMessage,
                MessageCategory = MotivationalMessageCategory.PinSiteCare,
                Body = "This is a locally simulated motivational message. This is a locally simulated motivational message. This is a locally simulated motivational message",
            };

            App.ViewModelLocator.Home.OnRemoteNotification(remoteNotification);
        }

        private void LocalNotificationCommandExecute()
        {
            var service = DependencyService.Get<INotificationManager>();

            DateTime dt = _sysUtility.Now.AddSeconds(10);            
            service.ScheduleLocalNotification(new LocalNotification()
            {
                DateTime = dt,
                Title = string.Format("Test local at {0}", dt.ToString()),
                Body = string.Format("Test Local body {0}", dt.ToString("HH:mm:ss")),
                NotificationType = LocalNotificationType.Generic
            });
        }


        private void TestCommandExecute()
        {
            throw new Exception("Test for unhandled exception");
        }

        private void ResetAllCommandExecute()
        {
            AppSettings.ResetToAnonymous();
            NotifyLoginState(false);
            if (_dbService.ResetToAnonymous())
            {
                Device.BeginInvokeOnMainThread(() =>
                    {
                        var service = DependencyService.Get<INotificationManager>();
                        service.DeleteAllLocalNotifications();
                    });
                LoadScheduledNotifications();
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {

                    App.Current.MainPage.DisplayAlert("Error", _dbService.LastException.Message, "OK");
                });
            }
        }

        private void SimulateCrashCommandExecute()
        {
            Crashes.GenerateTestCrash();
        }

    }
}

