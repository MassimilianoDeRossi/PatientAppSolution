//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Gms.Gcm;
//using Android.Util;
//using System.Collections.Generic;
//using Android.Media;
//using PatientApp.Droid.Services;
//using PatientApp.Services;

//namespace PatientApp.Droid
//{
//    [Service(Exported = false), IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE" })]
//    public class MyGcmListenerService : GcmListenerService
//    {
//        public override void OnMessageReceived(string from, Bundle extras)
//        {
//            // Extract the message received from GCM:
//            var values = new Dictionary<string, object>();
//            foreach (var key in extras.KeySet())
//            {
//                var value = extras.Get(key).ToString();
//                values.Add(key, value);
//            }

//            string msg = "";

//            System.Diagnostics.Debug.WriteLine("**debug** arrivato alla fine parte 1");

//            if (values.ContainsKey("type"))
//            {
//                var type = int.Parse((string)values["type"]);
//                var isSilent = false;

//                if (values.ContainsKey("message"))
//                {
//                    msg = (string)values["message"];
//                }

//                if (values.ContainsKey("silent"))
//                {
//                    isSilent = bool.Parse(values["silent"].ToString());
//                }

//                var remoteNotification = new RemoteNotification()
//                {
//                    Id = (System.DateTime.Now.Ticks % int.MaxValue).ToString(), //to avoid overflow,
//                    NotificationType = (RemoteNotificationType)type,
//                    Body = msg
//                };

//                if (values.ContainsKey("messagecategory")) //Add messagecategory if is not null
//                {
//                    var messageCategory = int.Parse((string)values["messagecategory"]);
//                    remoteNotification.MessageCategory = (MotivationalMessageCategory)messageCategory;
//                }

//                if (!isSilent)
//                {
//                    SendNotification(remoteNotification);
//                }

//                System.Diagnostics.Debug.WriteLine("**debug** arrivato alla fine ez");

//                Services.NotificationManagerImplementation.PushListener.OnRemoteNotification(remoteNotification);
//            }
//        }

//        private void SendNotification(RemoteNotification remoteNotification)
//        {
//            Context context = Android.App.Application.Context;
//            int idNotification = int.Parse(remoteNotification.Id);

//            ////var resultIntent = new Intent(context, typeof(MainActivity));
//            //var resultIntent = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
//            //resultIntent.AddFlags(ActivityFlags.SingleTop);
//            //resultIntent.PutExtra("Id", idNotification);

//            //var pendingIntent = PendingIntent.GetActivity(context, idNotification, resultIntent, PendingIntentFlags.OneShot | PendingIntentFlags.UpdateCurrent);

//            var resultIntent = new Intent(context, typeof(MainActivity));
//            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
//            resultIntent.PutExtra("Id", idNotification);

//            var pendingIntent = PendingIntent.GetActivity(context, idNotification,
//                resultIntent,
//                PendingIntentFlags.CancelCurrent/* | PendingIntentFlags.OneShot*/);

//            var notificationBuilder = new Notification.Builder(context)
//                .SetSmallIcon(PatientApp.Droid.Resource.Drawable.notification_icon)
//                .SetContentTitle("myHEXplan")
//                .SetContentText(remoteNotification.Body)
//                .SetAutoCancel(true)
//                .SetVibrate(new long[] { 300, 500, 300, 500 })
//                .SetLights(Android.Graphics.Color.White, 3000, 3000)
//                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
//                .SetContentIntent(pendingIntent);

//            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
//            {
//                // Using BigText notification style to support long message
//                var style = new Notification.BigTextStyle();
//                style.BigText(remoteNotification.Body);
//                notificationBuilder.SetStyle(style);
//            }

//            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
//            notificationManager.Notify(idNotification, notificationBuilder.Build());
//        }
//    }
//}
