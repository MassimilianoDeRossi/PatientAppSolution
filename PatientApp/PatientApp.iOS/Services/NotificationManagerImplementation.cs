using System;
using System.Collections.Generic;
using UIKit;

using PatientApp.Services;
using PatientApp.iOS.Services;
using PatientApp.iOS.Utilities;
using PatientApp.Utilities;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationManagerImplementation))]
namespace PatientApp.iOS.Services
{
    public class NotificationManagerImplementation : INotificationManager
    {
        public static string CurrentToken = null;

        public static IPushNotificationListener PushListener;
        public static ILocalNotificationListener LocalListener;

        public NotificationManagerImplementation()
        {

        }

        public string GetToken()
        {
            return CurrentToken;
        }

        public void Initialize(IPushNotificationListener pushListener, ILocalNotificationListener localListener)
        {
            PushListener = pushListener;
            LocalListener = localListener;
        }

        public void Register()
        {

        }        

        public void Unregister()
        {

        }

        public void DeleteLocalNotification(LocalNotification notification)
        {
            
        }

        public void DeleteAllLocalNotifications()
        {
            UIApplication.SharedApplication.CancelAllLocalNotifications();
        }

        /// <summary>
        /// Return all previously scheduled local notifications
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocalNotification> GetScheduledLocalNotifications()
        {
            var results = new List<LocalNotification>();

            foreach (var l in UIApplication.SharedApplication.ScheduledLocalNotifications)
            {                
                results.Add(l.ToLocalNotification());
            }            
            return results;
        }


        /// <summary>
        /// Schedule a local notification using the ios system 
        /// </summary>
        /// <param name="notification"></param>
        public void ScheduleLocalNotification(LocalNotification notification)
        {
            if (App.TestModel == null || !App.TestModel.TestModeOn || App.TestModel.SendNotifications)
            {
                // Convert app local notification to ios UILocalNotification and schedule it
                var iosNotification = notification.ToUILocalNotification();
                UIApplication.SharedApplication.ScheduleLocalNotification(iosNotification);

                AppLoggerHelper.LogEvent("LocalNotification", string.Format("Local notification scheduled on {0} - schedule datetime: {1} - type: {2}",
                            DateTime.Now,
                            iosNotification.FireDate.ToString(),
                            notification.NotificationType.ToString())
                            , System.Diagnostics.TraceLevel.Info);
            }
        }

    }
}