using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Util;
using Firebase.Messaging;
using System.Collections.Generic;
using PatientApp.Services;

namespace PatientApp.Droid.Notifications
{
    /// <summary>
    /// Intent parameters name for remote notifications
    /// </summary>
    public static class RemoteNotificationIntent
    {
        public const string SILENT = "silent";
        public const string MESSAGE = "message";
        public const string TYPE = "type";
        public const string MESSAGE_CATEGORY = "messagecategory";
        public const string ID = "remote_id";
    }

    [Service(Exported = false)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        private readonly long[] VIBRATION_PATTERN = new long[] { 0, 200, 200, 400, 200, 1000 };
        private readonly string REMOTE_NOTIFICATION_CHANNEL = "remote";
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

        public override void OnMessageReceived(RemoteMessage message)
        {
            var values = message.Data;
            var remoteNotification = CreateRemoteNotification(values);
            if (remoteNotification != null && !String.IsNullOrWhiteSpace(remoteNotification.Body))
            {
                if (!IsSilent(values))
                {
                    SendNotification(remoteNotification);
                }
                Services.NotificationManagerImplementation.PushListener.OnRemoteNotification(remoteNotification); //this is in homeviewmodel construcotr as well when restore app killed
            }
        }

        /// <summary>
        /// Check if notification is silent. If is not specified it will be not silent.
        /// </summary>
        private bool IsSilent(IDictionary<string, string> values)
        {
            var isSilent = false; //default value
            if (values.ContainsKey(RemoteNotificationIntent.SILENT))
            {
                bool.TryParse(values[RemoteNotificationIntent.SILENT], out isSilent);
            }
            return isSilent;
        }

        /// <summary>
        /// Create the remote notification. Returns null if creation failed (notification type is not present or invalid).
        /// </summary>
        private RemoteNotification CreateRemoteNotification(IDictionary<string, string> values)
        {
            if (values.ContainsKey(RemoteNotificationIntent.TYPE))
            {
                RemoteNotificationType type;
                if (Enum.TryParse(values[RemoteNotificationIntent.TYPE], out type))
                {
                    string msg;
                    MotivationalMessageCategory mmType = MotivationalMessageCategory.Generic; //default

                    values.TryGetValue(RemoteNotificationIntent.MESSAGE, out msg);

                    if (values.ContainsKey(RemoteNotificationIntent.MESSAGE_CATEGORY))
                    {
                        Enum.TryParse(values[RemoteNotificationIntent.MESSAGE_CATEGORY], out mmType);
                    }

                    return new RemoteNotification()
                    {
                        Id = (System.DateTime.Now.Ticks % int.MaxValue).ToString(), //to avoid overflow,
                        NotificationType = type,
                        Body = msg,
                        MessageCategory = mmType,
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// Send Notification to operative system
        /// </summary>
        private void SendNotification(RemoteNotification remoteNotification)
        {
            Context context = Application.Context;
            var notificationManager = NotificationManager.FromContext(context);

            int notificationId = int.Parse(remoteNotification.Id);

            var resultIntent = new Intent(context, typeof(MainActivity));
            resultIntent.SetFlags(ActivityFlags.SingleTop);
            resultIntent.PutExtra(RemoteNotificationIntent.ID, notificationId.ToString());
            resultIntent.PutExtra(RemoteNotificationIntent.TYPE, (int)remoteNotification.NotificationType);
            resultIntent.PutExtra(RemoteNotificationIntent.MESSAGE_CATEGORY, (int)remoteNotification.MessageCategory);
            resultIntent.PutExtra(RemoteNotificationIntent.MESSAGE, remoteNotification.Body);

            var pendingIntent = PendingIntent.GetActivity(context, notificationId, resultIntent, PendingIntentFlags.CancelCurrent);
            var notification = CreateNotificationBasedOnApiVersion(context, remoteNotification, pendingIntent, notificationManager);

            notificationManager.Notify(notificationId, notification);
        }

        /// <summary>
        /// Create a Notification based on API version to avoid to use deprecated methods on new devices
        /// </summary>
        private Notification CreateNotificationBasedOnApiVersion(Context context, RemoteNotification remoteNotification, PendingIntent pendingIntent, NotificationManager notificationManager)
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                return CreateNotificationApiHigherOrEqualsThan26(remoteNotification, pendingIntent, context, notificationManager, VIBRATION_PATTERN);
            }
            else
            {
                return CreateNotificationApiLessThan26(remoteNotification, pendingIntent, context, VIBRATION_PATTERN);
            }
        }

        /// <summary>
        /// Create a notification using API 25- (not using Notification Channel but deprecated methods)
        /// </summary>
        private Notification CreateNotificationApiLessThan26(RemoteNotification remoteNotification, PendingIntent pendingIntent, Context context, long[] vibrationPattern)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var notificationBuilder = new Notification.Builder(context)
                .SetSmallIcon(NotificationIcon)
                .SetContentTitle(GetNotificationTitle(remoteNotification.NotificationType))
                .SetContentIntent(pendingIntent)
                .SetContentText(remoteNotification.Body)
                .SetAutoCancel(true)
                .SetVibrate(vibrationPattern)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetPriority((int)NotificationPriority.High);
#pragma warning restore CS0618 // Type or member is obsolete

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                var style = new Notification.BigTextStyle(); // Using BigText notification style to support long message
                style.BigText(remoteNotification.Body);
                notificationBuilder.SetStyle(style);
            }

            return notificationBuilder.Build();
        }

        /// <summary>
        /// Create a notification using API 26+ (using Notification Channel)
        /// </summary>
        private Notification CreateNotificationApiHigherOrEqualsThan26(RemoteNotification remoteNotification, PendingIntent pendingIntent, Context context, NotificationManager notificationManager, long[] vibrationPattern)
        {
            CheckAndCreateNotificationChannel(notificationManager, REMOTE_NOTIFICATION_CHANNEL, vibrationPattern);

            var notificationBuilder = new Notification.Builder(context, REMOTE_NOTIFICATION_CHANNEL)
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetContentTitle(GetNotificationTitle(remoteNotification.NotificationType))
                .SetContentIntent(pendingIntent)
                .SetContentText(remoteNotification.Body)
                .SetAutoCancel(true)
                .SetStyle(new Notification.BigTextStyle().BigText(remoteNotification.Body));

            return notificationBuilder.Build();
        }

        /// <summary>
        /// Create if not exists, the notification channel used to send remote notifications. This has been introduced with API 26.
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
        private string GetNotificationTitle(RemoteNotificationType type)
        {
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.N)
            {
                return DEFAULT_NOTIFICATION_TITLE;
            }
            switch (type)
            {
                case RemoteNotificationType.DeviceChanged:
                    return "Prescription Transferred to Another Device";
                case RemoteNotificationType.Generic:
                    return "Generic";
                case RemoteNotificationType.MotivationalMessage:
                    return "Insight Message";
                case RemoteNotificationType.PinSiteCare:
                    return "Pin Site Care";
                case RemoteNotificationType.Prescription:
                    return "Prescription Updated";
                case RemoteNotificationType.WakeUp:
                    return "Wake Up";
                default:
                    return DEFAULT_NOTIFICATION_TITLE;
            }
        }
    }
}