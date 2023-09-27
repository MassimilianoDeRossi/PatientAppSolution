using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Foundation;
using UIKit;

using PatientApp.Services;

namespace PatientApp.iOS.Utilities
{
    public static class UILocalNotificationExtensions
    {             
        public static LocalNotification ToLocalNotification(this UILocalNotification iosNotification)
        {
            var info = iosNotification.UserInfo.ToDictionary();
            var notification = new LocalNotification()
            {
                DateTime = iosNotification.FireDate.ToDateTime(),
                Title = iosNotification.AlertTitle,
                Body = iosNotification.AlertBody,
            };

            if (info.ContainsKey("PrescriptionId"))
                notification.PrescriptionId = Guid.Parse(info["PrescriptionId"]);

            if (info.ContainsKey("EntityId"))
                notification.EntityId = Guid.Parse(info["EntityId"]);

            if (info.ContainsKey("NotificationType") && info["NotificationType"] != null)
            {
                LocalNotificationType nType;
                if (Enum.TryParse<LocalNotificationType>(info["NotificationType"], out nType))
                {
                    notification.NotificationType = nType;
                }
            }
            return notification;
        }

        public static UILocalNotification ToUILocalNotification(this LocalNotification notification)
        {
            var iosNotification = new UILocalNotification();

            var userInfo = new Dictionary<string, string>();
            userInfo.Add("PrescriptionId", notification.PrescriptionId.ToString());
            userInfo.Add("EntityId", notification.EntityId.ToString());
            userInfo.Add("NotificationType", notification.NotificationType.ToString());

            // set the fire date (the date time in which it will fire)            
            iosNotification.FireDate = notification.DateTime.ToNsDate();
            // configure the alert            
            iosNotification.AlertTitle = notification.Title;
            iosNotification.AlertBody = notification.Body;
            //iosNotification.HasAction = true;
            //iosNotification.AlertAction = notification.NotificationType.ToString();
            // modify the badge
            iosNotification.ApplicationIconBadgeNumber = 0;
            // set the sound to be the default sound
            iosNotification.SoundName = UILocalNotification.DefaultSoundName;

            var info = new NSMutableDictionary();
            if (userInfo != null)
            {
                foreach (var keyPair in userInfo)
                {
                    info.SetValueForKey(new NSString(keyPair.Value), new NSString(keyPair.Key));
                }
            }
            iosNotification.UserInfo = info;

            return iosNotification;
        }


        //private static NSDate DateTimeToNSDate(DateTime date)
        //{
        //    DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
        //        new DateTime(2001, 1, 1, 0, 0, 0));
        //    return NSDate.FromTimeIntervalSinceReferenceDate(
        //        (date - reference).TotalSeconds);
        //}

    }
}
