using Android.App;
using Android.Content;
using Android.Media;
using PatientApp.Droid.Services;
using PatientApp.Services;

namespace PatientApp.Droid.Notifications
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new string[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted, "android.intent.action.QUICKBOOT_POWERON", "com.htc.intent.action.QUICKBOOT_POWERON" })]
    public class AlarmReceiver : BroadcastReceiver
    {
        private readonly long[] VIBRATION_PATTERN = new long[] { 0, 200, 200, 400, 200, 1000 };
        private readonly string LOCAL_NOTIFICATION_CHANNEL = "local";
        private readonly string DEFAULT_NOTIFICATION_TITLE = "myHEXplan";

        private int NotificationIcon
        {
            get
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
                {
                    return Resource.Drawable.notification_icon;
                }
                return Resource.Drawable.notification_icon_white;
            }
        }

        public override void OnReceive(Context context, Intent intent)
        {
            //if reboot device, reschedule notifications
            if (intent.Action == Intent.ActionBootCompleted ||
                intent.Action == Intent.ActionLockedBootCompleted ||
                intent.Action == "android.intent.action.QUICKBOOT_POWERON" ||
                intent.Action == "com.htc.intent.action.QUICKBOOT_POWERON")
            {
                Intent pushIntent = new Intent(context, typeof(RebootRescheduler));
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    context.StartForegroundService(pushIntent);
                }
                else
                {
                    context.StartService(pushIntent);
                }
            }
            else
            {
                var localNotification = CreateLocalNotificationFromIntent(intent);
                var notificationId = intent.GetIntExtra(LocalNotificationIntent.ID, (int)(System.DateTime.Now.Ticks % int.MaxValue)); //to avoid overflow
                SendNotification(localNotification, notificationId, context);
                NotificationManagerImplementation.RemoveAlarm(context, notificationId);
                NotificationManagerImplementation.LocalListener.OnLocalNotification(localNotification);
            }
        }

        /// <summary>
        /// Send Notification to operative system
        /// </summary>
        private void SendNotification(LocalNotification localNotification, int notificationId, Context context)
        {
            var notificationManager = NotificationManager.FromContext(context);

            var resultIntent = new Intent(context, typeof(MainActivity));
            resultIntent.SetFlags(ActivityFlags.SingleTop);
            resultIntent.PutExtra(LocalNotificationIntent.ID, notificationId);
            resultIntent.PutExtra(LocalNotificationIntent.MESSAGE, localNotification.Body);
            resultIntent.PutExtra(LocalNotificationIntent.TITLE, localNotification.Title);
            resultIntent.PutExtra(LocalNotificationIntent.TYPE, (int)localNotification.NotificationType);

            var pendingIntent = PendingIntent.GetActivity(context, notificationId, resultIntent, PendingIntentFlags.CancelCurrent);
            var notification = CreateNotificationBasedOnApiVersion(context, localNotification, pendingIntent, notificationManager);

            notificationManager.Notify(notificationId, notification);
        }

        /// <summary>
        /// Create a LocalNotification object from intent. If the message is empty, it swaps title with body and the title become myHexPlan
        /// </summary>
        private LocalNotification CreateLocalNotificationFromIntent(Intent intent)
        {
            var message = intent.GetStringExtra(LocalNotificationIntent.MESSAGE);
            var title = intent.GetStringExtra(LocalNotificationIntent.TITLE);
            var type = (LocalNotificationType)intent.GetIntExtra(LocalNotificationIntent.TYPE, 10);

            if (System.String.IsNullOrWhiteSpace(message))
            {
                message = title;
                title = GetNotificationTitle(type);
            }

            return new LocalNotification()
            {
                Body = message,
                Title = title,
                NotificationType = type
            };
        }

        /// <summary>
        /// Create a Notification based on API version to avoid to use deprecated methods on new devices
        /// </summary>
        private Notification CreateNotificationBasedOnApiVersion(Context context, LocalNotification localNotification, PendingIntent pendingIntent, NotificationManager notificationManager)
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                return CreateNotificationApiHigherOrEqualsThan26(localNotification, pendingIntent, context, notificationManager, VIBRATION_PATTERN);
            }
            else
            {
                return CreateNotificationApiLessThan26(localNotification, pendingIntent, context, VIBRATION_PATTERN);
            }
        }

        /// <summary>
        /// Create a notification using API 25- (not using Notification Channel but deprecated methods)
        /// </summary>
        private Notification CreateNotificationApiLessThan26(LocalNotification localNotification, PendingIntent pendingIntent, Context context, long[] vibrationPattern)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var notificationBuilder = new Notification.Builder(context)
                .SetSmallIcon(NotificationIcon)
                .SetContentTitle(localNotification.Title)
                .SetContentIntent(pendingIntent)
                .SetContentText(localNotification.Body)
                .SetAutoCancel(true)
                .SetVibrate(vibrationPattern)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetPriority((int)NotificationPriority.High);
#pragma warning restore CS0618 // Type or member is obsolete

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                var style = new Notification.BigTextStyle(); // Using BigText notification style to support long message
                style.BigText(localNotification.Body);
                notificationBuilder.SetStyle(style);
            }

            return notificationBuilder.Build();
        }

        /// <summary>
        /// Create a notification using API 26+ (using Notification Channel)
        /// </summary>
        private Notification CreateNotificationApiHigherOrEqualsThan26(LocalNotification localNotification, PendingIntent pendingIntent, Context context, NotificationManager notificationManager, long[] vibrationPattern)
        {
            CheckAndCreateNotificationChannel(notificationManager, LOCAL_NOTIFICATION_CHANNEL, vibrationPattern);

            var notificationBuilder = new Notification.Builder(context, LOCAL_NOTIFICATION_CHANNEL)
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetContentTitle(localNotification.Title)
                .SetContentIntent(pendingIntent)
                .SetContentText(localNotification.Body)
                .SetAutoCancel(true)
                .SetStyle(new Notification.BigTextStyle().BigText(localNotification.Body));

            return notificationBuilder.Build();
        }

        /// <summary>
        /// Create if not exists, the notification channel used to send local notifications. This has been introduced with API 26.
        /// </summary>
        private void CheckAndCreateNotificationChannel(NotificationManager notificationManager, string channelName, long[] vibrationPattern)
        {
            var channel = notificationManager.GetNotificationChannel(channelName);
            if (channel == null)
            {
                channel = new NotificationChannel(channelName, channelName, NotificationImportance.High);
                channel.EnableVibration(true);
                channel.SetVibrationPattern(vibrationPattern);
                channel.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification), new AudioAttributes.Builder().Build());
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        /// <summary>
        /// Generate notification title. On android 7+ the title of the app is already displayed by os, so we change title to message category
        /// </summary>
        private string GetNotificationTitle(LocalNotificationType type)
        {
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.N)
            {
                return DEFAULT_NOTIFICATION_TITLE;
            }
            switch (type)
            {
                case LocalNotificationType.Generic:
                    return "Generic";
                case LocalNotificationType.PinSiteCareReminder:
                    return "Pin Site Care";
                case LocalNotificationType.PrescriptionUpdated:
                    return "Prescription Updated";
                case LocalNotificationType.StrutAdjustmentReminder:
                    return "Struts Adjustment";
                case LocalNotificationType.SyncReminder:
                    return "Sync Reminder";
                default:
                    return DEFAULT_NOTIFICATION_TITLE;
            }
        }
    }
}