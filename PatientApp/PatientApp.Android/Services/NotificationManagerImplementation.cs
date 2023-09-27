using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;

using PatientApp.Droid.Notifications;
using System.Text;

using PatientApp.Services;
using PatientApp.Utilities;
using PatientApp.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationManagerImplementation))]
namespace PatientApp.Droid.Services
{
    /// <summary>
    /// Intent parameters name for local notifications
    /// </summary>
    public static class LocalNotificationIntent
    {
        public const string ID = "local_id";
        public const string TITLE = "title";
        public const string MESSAGE = "message";
        public const string TYPE = "type";
        public const string DATE = "date";
    }

    /// <summary>
    /// Contains data to restore notifications after reboot
    /// </summary>
    public class LocalNotificationToRestore
    {
        public DateTime DateTime { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public LocalNotificationType NotificationType { get; set; }
        public int Id { get; set; }
    }

    public class NotificationManagerImplementation : INotificationManager
    {
        public static string CurrentToken = null;

        public static IPushNotificationListener PushListener;
        public static ILocalNotificationListener LocalListener;

        private const string IDS_LIST = "IDS_LIST";
        private const string PREF_NAME_IDS = "notificationList";
        private const int NUMBER_TO_DELETE_ZEROS = 100000000;


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
            Context context = Android.App.Application.Context;
            Intent intent = new Intent(context, typeof(AlarmReceiver));
            List<int> listAlarmIds = GetAlarmIds(context);

            foreach (int idAlarm in listAlarmIds)
            {
                CancelAlarm(context, intent, idAlarm);
            }
        }

        /// <summary>
        /// Return all previously scheduled local notifications. On Android it will valorize only date and type.
        /// </summary>
        public IEnumerable<LocalNotification> GetScheduledLocalNotifications()
        {
            Context context = Android.App.Application.Context;
            var results = new List<LocalNotification>();
            List<int> idsAlarms = GetAlarmIds(context);
            Intent intent = new Intent(context, typeof(AlarmReceiver));

            foreach (int id in idsAlarms)
            {
                if (HasAlarm(context, intent, id)) //check if it exists for real
                {
                    LocalNotification notification = new LocalNotification()
                    {
                        DateTime = GetDateFromId(id),
                        NotificationType = GetTypeFromId(id),
                    };
                    results.Add(notification);
                }
            }

            return results;
        }

        /// <summary>
        /// Schedule a local notification on Android
        /// </summary>
        public void ScheduleLocalNotification(LocalNotification notification)
        {
            if (App.TestModel == null || !App.TestModel.TestModeOn || App.TestModel.SendNotifications)
            {
                var context = Android.App.Application.Context;
                AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

                int notificationId = CreateId(notification, context);
                var alarmIntent = CreateAlarmIntent(notification, notificationId, context);
                var pendingIntent = PendingIntent.GetBroadcast(context, notificationId, alarmIntent, PendingIntentFlags.UpdateCurrent);

                ScheduleAlarmBasedOnAPIVersion(alarmManager, notification, pendingIntent);
                SaveAlarmIdAndDetails(context, notificationId, notification);

                AppLoggerHelper.LogEvent("LocalNotification", string.Format("Local notification scheduled on {0} - schedule datetime: {1} - type: {2}",
                            DateTime.Now,
                            notification.DateTime.ToString(),
                            notification.NotificationType.ToString())
                            , System.Diagnostics.TraceLevel.Info);
            }
        }

        /// <summary>
        /// Schedule a notification without save anything in PreferenceManager. Used only after reboot to restore notifications lost.
        /// </summary>
        public void ScheduleLocalNotificationForReboot(LocalNotificationToRestore notificationToRestore)
        {
            if (App.TestModel == null || !App.TestModel.TestModeOn || App.TestModel.SendNotifications)
            {
                Console.WriteLine("myHEXplan - Trying to rescheduling a notification with id " + notificationToRestore.Id);

                var context = Android.App.Application.Context;
                AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

                int notificationId = notificationToRestore.Id;
                var notification = new LocalNotification()
                {
                    Body = notificationToRestore.Body,
                    DateTime = notificationToRestore.DateTime,
                    NotificationType = notificationToRestore.NotificationType,
                    Title = notificationToRestore.Title,
                };

                var alarmIntent = CreateAlarmIntent(notification, notificationId, context);
                var pendingIntent = PendingIntent.GetBroadcast(context, notificationId, alarmIntent, PendingIntentFlags.UpdateCurrent);

                ScheduleAlarmBasedOnAPIVersion(alarmManager, notification, pendingIntent);

                AppLoggerHelper.LogEvent("ScheduleLocalNotificationFromReboot", string.Format("Local notification scheduled after reboot on {0} - schedule datetime: {1} - type: {2}",
                        DateTime.Now,
                        notification.DateTime.ToString(),
                        notification.NotificationType.ToString())
                        , System.Diagnostics.TraceLevel.Info);
            }
        }

        /// <summary>
        /// Create a new intent of AlarmReceiver with data valorized
        /// </summary>
        private Intent CreateAlarmIntent(LocalNotification notification, int notificationId, Context context)
        {
            var alarmIntent = new Intent(context, typeof(AlarmReceiver));
            alarmIntent.PutExtra(LocalNotificationIntent.ID, notificationId);
            alarmIntent.PutExtra(LocalNotificationIntent.TITLE, notification.Title);
            alarmIntent.PutExtra(LocalNotificationIntent.MESSAGE, notification.Body);
            alarmIntent.PutExtra(LocalNotificationIntent.TYPE, (int)notification.NotificationType);
            alarmIntent.PutExtra(LocalNotificationIntent.DATE, notification.DateTime.Ticks);
            return alarmIntent;
        }

        /// <summary>
        /// Schedule a notification based on API version
        /// </summary>
        private void ScheduleAlarmBasedOnAPIVersion(AlarmManager alarmManager, LocalNotification notification, PendingIntent pendingIntent)
        {
            Java.Util.Calendar calendar = Java.Util.Calendar.Instance;
            calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
            calendar.Set(notification.DateTime.Year, notification.DateTime.Month - 1, notification.DateTime.Day, notification.DateTime.Hour, notification.DateTime.Minute, 0);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);
            }
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                alarmManager.SetExact(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);
            }
            else
            {
                alarmManager.Set(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);
            }
        }

        /// <summary>
        /// Save a single notificationId on PreferenceManager (persistent) and save also data used to restore a notification after reboot
        /// </summary>
        private void SaveAlarmIdAndDetails(Context context, int id, LocalNotification notification)
        {
            List<int> idsAlarms = GetAlarmIds(context);

            if (!idsAlarms.Contains(id))
            {
                idsAlarms.Add(id);
                SaveIdsInPreferences(context, idsAlarms);
            }

            SaveDetailsInPreferences(context, id, notification);
        }

        /// <summary>
        /// Get id list of scheduled notifications from PreferenceManager
        /// </summary>
        private static List<int> GetAlarmIds(Context context)
        {
            List<int> ids = new List<int>();

            ISharedPreferences prefs = context.GetSharedPreferences(PREF_NAME_IDS, FileCreationMode.Private);
            string idsListWithComma = prefs.GetString(IDS_LIST, "");
            string[] idsListSplitted = idsListWithComma.Split(',');
            for (int i = 0; i < idsListSplitted.Length - 1; i++)
            {
                ids.Add(int.Parse(idsListSplitted[i]));
            }

            return ids;
        }

        /// <summary>
        /// Get all scheduled notification data. Used to restore notifications after reboot.
        /// </summary>
        public IEnumerable<LocalNotificationToRestore> GetScheduledNotificationForReboot()
        {
            Context context = Android.App.Application.Context;
            List<int> ids = GetAlarmIds(context);

            foreach (var id in ids)
            {
                var notification = new LocalNotificationToRestore();
                ISharedPreferences prefs = context.GetSharedPreferences(id.ToString(), FileCreationMode.Private);

                notification.DateTime = new DateTime(prefs.GetLong(LocalNotificationIntent.DATE, 0));
                notification.Body = prefs.GetString(LocalNotificationIntent.MESSAGE, "");
                notification.NotificationType = (LocalNotificationType)prefs.GetInt(LocalNotificationIntent.TYPE, 0);
                notification.Title = prefs.GetString(LocalNotificationIntent.TITLE, "");
                notification.Id = id;

                yield return notification;
            }
        }

        /// <summary>
        /// Save all notificationId on PreferenceManager (persistent)
        /// </summary>
        private static void SaveIdsInPreferences(Context context, List<int> lstIds)
        {
            var strBuilder = new StringBuilder();
            foreach (int idAlarm in lstIds)
            {
                strBuilder.Append(idAlarm + ",");
            }

            string idsListStr = strBuilder.ToString();

            ISharedPreferences prefs = context.GetSharedPreferences(PREF_NAME_IDS, FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.Clear();
            editor.PutString(IDS_LIST, idsListStr);
            editor.Commit();
        }


        /// <summary>
        /// Save notification details using shared prefereces (persistent), used to restore notification after reboot
        /// </summary>
        private void SaveDetailsInPreferences(Context context, int id, LocalNotification notification)
        {
            ISharedPreferences prefs = context.GetSharedPreferences(id.ToString(), FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.Clear();
            editor.PutLong(LocalNotificationIntent.DATE, notification.DateTime.Ticks);
            editor.PutString(LocalNotificationIntent.ID, id.ToString());
            editor.PutString(LocalNotificationIntent.MESSAGE, notification.Body);
            editor.PutString(LocalNotificationIntent.TITLE, notification.Title);
            editor.PutInt(LocalNotificationIntent.TYPE, (int)notification.NotificationType);
            editor.Commit();
        }

        /// <summary>
        /// Check if a notification is scheduled
        /// </summary>
        private bool HasAlarm(Context context, Intent intent, int notificationId)
        {
            return PendingIntent.GetBroadcast(context, notificationId, intent, PendingIntentFlags.NoCreate) != null;
        }

        /// <summary>
        /// Cancel a scheduled notificaiton
        /// </summary>
        private void CancelAlarm(Context context, Intent intent, int notificationId)
        {
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, notificationId, intent, PendingIntentFlags.CancelCurrent);
            alarmManager.Cancel(pendingIntent);
            pendingIntent.Cancel();

            RemoveAlarm(context, notificationId);
        }

        /// <summary>
        /// Remove the alarm id and details from SharedPreference
        /// </summary>
        public static void RemoveAlarm(Context context, int id)
        {
            List<int> idsAlarms = GetAlarmIds(context);
            idsAlarms.Remove(id);
            SaveIdsInPreferences(context, idsAlarms);
            RemoveAlarmDetailed(context, id);
        }

        /// <summary>
        /// Remove alarm details created to restore notification in case of reboot
        /// </summary>
        public static void RemoveAlarmDetailed(Context context, int id)
        {
            var settings = context.GetSharedPreferences(id.ToString(), FileCreationMode.Private);
            settings.Edit().Clear().Commit();
        }

        /// <summary>
        /// Create a unique id for a single notification, where will be possible recovery time and type.
        /// If there are more than 20 notifications with same type and time, recovered data will be wrong but the notification will work correctly.
        /// </summary>
        private int CreateId(LocalNotification notification, Context context)
        {
            //to get data for debug the id is created by union of scheduled time and type
            //Ticks 28 March 2018 10:59:00 -> 636578315400000000
            //we take 6365783154 (so we're not considering seconds and ms because they're all always 0) 
            //remove first 3 number at the start (636) (because it will change after a lot of years)
            //add insteod of those 3 number, 2 number autoincremental to avoid conflicts and for third number the notification type.

            long ticks = notification.DateTime.Ticks / NUMBER_TO_DELETE_ZEROS; //delete all zeros
            int idTemp = (int)(1000000 * (int)notification.NotificationType + ticks % 10000000);

            List<int> idsAlarms = GetAlarmIds(context);
            int countWithSameTypeAndDate;
            int IdToReturn = 0;
            for (countWithSameTypeAndDate = 0; countWithSameTypeAndDate < 21; countWithSameTypeAndDate++)
            {
                IdToReturn = idTemp + 100000000 * countWithSameTypeAndDate;
                if (!idsAlarms.Contains(IdToReturn)) break;
            }

            if (countWithSameTypeAndDate > 20)
            {
                //overflow. This will be displayed wrong in debug mode, but notification will work correctly.
                return (int)(DateTime.Now.Ticks % int.MaxValue);
            }

            return IdToReturn;
        }

        /// <summary>
        /// Recover the date from id, if it is generated by CreateId method
        /// </summary>
        private DateTime GetDateFromId(int id)
        {
            long ticks = (long)(id % 10000000) * NUMBER_TO_DELETE_ZEROS;
            long nowTicks = DateTime.Now.Ticks;
            nowTicks -= nowTicks % ((long)10000000 * NUMBER_TO_DELETE_ZEROS);
            nowTicks += ticks;
            return new DateTime(nowTicks);
        }

        /// <summary>
        /// Recover the notificaiton type from id, if it is generated by CreateId method
        /// </summary>
        private LocalNotificationType GetTypeFromId(int id)
        {
            long type = (id / 10000000) % 10;
            return (LocalNotificationType)(type * 10);
        }
    }
}