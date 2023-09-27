using System;
using System.Collections.Generic;

namespace PatientApp.Services
{
    public enum LocalNotificationType
    {
        Generic = 10,
        SyncReminder = 20,
        StrutAdjustmentReminder = 30,
        PinSiteCareReminder = 40,
        PrescriptionUpdated = 50,
    }

    public enum RemoteNotificationType
    {
        /// <summary>
        /// Generic
        /// </summary>
        Generic = 10,

        /// <summary>
        /// Silent notification to wake up the device
        /// </summary>
        WakeUp = 20,

        /// <summary>
        /// New or modify PinSiteCare
        /// </summary>
        PinSiteCare = 30,

        /// <summary>
        /// New or modify MotivationalMessage
        /// </summary>
        MotivationalMessage = 40,

        /// <summary>
        /// Update on presscriptions (Revoke, Update or Add)
        /// </summary>
        Prescription = 50,

        /// <summary>
        /// User has changed device 
        /// </summary>
        DeviceChanged = 60
    }

    public enum MotivationalMessageCategory
    {

        StrutsAdjustment = 10,

        PinSiteCare = 20,

        PhysicalTherapy = 30,

        Generic = 40
    }

    /// <summary>
    /// Supported Intallation Platforms for Notification Hub
    /// </summary>
    public enum NotificationPlatformType
    {
        /// <summary>
        /// WNS Installation Platform (Windows Push Notification Services)
        /// </summary>
        Wns = 10,
        /// <summary>
        /// APNS Installation Platform (Apple Push Notification Service)
        /// </summary>
        Apns = 20,
        /// <summary>
        /// MPNS Installation Platform (Microsoft Push Notification Service)
        /// </summary>
        Mpns = 30,
        /// <summary>
        /// GCM Installation Platform (Google Cloud Message)
        /// </summary>
        Gcm = 40,
        /// <summary>
        /// ADM Installation Platform (Amazon Device Messaging)
        /// </summary>
        Adm = 50
    }

    /// <summary>
    /// Local notification class 
    /// </summary>
    public class LocalNotification
    {
        /// <summary>
        /// Scheduled datetime
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// The title of notification
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The body text of notification
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The case prescription identifier (has a value when the notificaton is related to a specific case)
        /// </summary>
        public Guid PrescriptionId { get; set; }

        /// <summary>
        /// The local database related entity identifier (reminder item for example)
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// The type of the local notification
        /// </summary>
        public LocalNotificationType NotificationType { get; set; }

        public LocalNotification()
        {

        }
    }

    /// <summary>
    /// Remote (push) notification
    /// </summary>
    public class RemoteNotification
    {
        /// <summary>
        /// Unique identifier 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Body text of notification
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Motivational message category (has value when notification is of MotivationalMessage type)
        /// </summary>
        public MotivationalMessageCategory MessageCategory { get; set; }

        /// <summary>
        /// The type of the remote notification
        /// </summary>
        public RemoteNotificationType NotificationType { get; set; }

        public RemoteNotification()
        {

        }
    }

    /// <summary>
    /// Interface to implement for listening to Remote (Push) Notifications
    /// </summary>
    public interface IPushNotificationListener
    {
        void OnRemoteNotification(RemoteNotification remoteNotification);
        void OnRegistered(string Token);
        void OnUnregistered();
        void OnError(string message);
    }

    /// <summary>
    /// Interface to implement for listening to local Notifications
    /// </summary>
    public interface ILocalNotificationListener
    {
        void OnLocalNotification(LocalNotification notification);
    }

    /// <summary>
    /// Dependency service used to manage native local and remote notififcatons 
    /// </summary>
    public interface INotificationManager
    {
        void Initialize(IPushNotificationListener pushListener, ILocalNotificationListener localListener);
        string GetToken();
        void Register();
        void Unregister();
        void ScheduleLocalNotification(LocalNotification notification);
        void DeleteLocalNotification(LocalNotification notification);
        void DeleteAllLocalNotifications();
        IEnumerable<LocalNotification> GetScheduledLocalNotifications();
    }

}
