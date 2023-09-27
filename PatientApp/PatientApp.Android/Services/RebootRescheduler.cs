using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace PatientApp.Droid.Services
{
    /// <summary>
    /// Service used to reschedule notifications after device reboot.
    /// </summary>
    [Service]
    public class RebootRescheduler : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            Console.WriteLine("myHEXplan - RebootRescheduler started");

            base.OnCreate();

            var notificationManager = new NotificationManagerImplementation();
            foreach (var notification in notificationManager.GetScheduledNotificationForReboot())
            {
                notificationManager.ScheduleLocalNotificationForReboot(notification);
            }
        }
    }
}