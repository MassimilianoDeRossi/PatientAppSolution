using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Gms.Gcm;
using Android.Gms.Iid;
using Firebase.Messaging;
using Firebase.Iid;

using PatientApp.Utilities;

namespace PatientApp.Droid
{
    // This intent service receives the registration token from GCM:
    [Service(Exported = false)]
    class RegistrationIntentService : IntentService
    {
        // Notification topics that I subscribe to:
        static readonly string[] Topics = { "global" };

        // Create the IntentService, name the worker thread for debugging purposes:
        public RegistrationIntentService() : base("RegistrationIntentService")
        { }

        // OnHandleIntent is invoked on a worker thread:
        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                // Ensure that the request is atomic:
                lock (FirebaseInstanceId.Instance)
                {
                    // Request a registration token:
                    var instanceID = FirebaseInstanceId.Instance;
                    var token = instanceID.Token;

                    if (token != null)
                    {

                        // Log the registration token that was returned from GCM:
                        Log.Info("RegistrationIntentService", "FCM Registration");

                        if (Services.NotificationManagerImplementation.PushListener != null)
                            Services.NotificationManagerImplementation.PushListener.OnRegistered(token);

                        // Subscribe to receive notifications:
                        SubscribeToTopics(Topics);
                        AppLoggerHelper.LogEvent("Token Generated", "Token generated successfully", System.Diagnostics.TraceLevel.Info);
                    }
                    else
                    {
                        AppLoggerHelper.LogEvent("Token Generation Failed", "Token generation failed", System.Diagnostics.TraceLevel.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLoggerHelper.LogException(ex, "OnHandleIntent RegistrationIntentService Exception", System.Diagnostics.TraceLevel.Error);
            }
        }


        // Subscribe to topics to receive notifications from the app server:
        void SubscribeToTopics(string[] topics)
        {
            lock (FirebaseMessaging.Instance)
            {
                foreach (var topic in topics)
                {
                    FirebaseMessaging.Instance.SubscribeToTopic(topic);
                }
            }
        }
    }
}
