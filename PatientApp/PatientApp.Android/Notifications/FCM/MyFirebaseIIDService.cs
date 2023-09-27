using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using Android.Content;
using Firebase.Messaging;

namespace PatientApp.Droid.Notifications
{
    [Service(Exported = false)]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            var intent = new Intent(this, typeof(RegistrationIntentService));
            StartService(intent);
        }
    }
}