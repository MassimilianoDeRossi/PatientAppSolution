using Foundation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PatientApp.iOS.Utilities
{
    public static class NSDateExtensions
    {
        /// <summary>The NSDate from Xamarin takes a reference point form January 1, 2001, at 12:00</summary>
        /// <remarks>avascript 
        /// It also has calls for NIX reference point 1970 but appears to be problematic
        /// </remarks>
        private static DateTime _nsRef = new DateTime(2001, 1, 1, 0, 0, 0); // last zero is millisecond       

        /// <summary>Convert a DateTime to NSDate</summary>
        /// <param name="dt">The DateTime to convert</param>
        /// <returns>An NSDate</returns>
        public static NSDate ToNsDate(this DateTime dt)
        {
            var localDt = dt;
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(localDt))
            {
                localDt = localDt.AddHours(-1);
            }
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(_nsRef);
            return NSDate.FromTimeIntervalSinceReferenceDate((localDt - reference).TotalSeconds);            
        }


        /// <summary>Convert an NSDate to DateTime</summary>
        /// <param name="nsDate">The NSDate to convert</param>
        /// <returns>A DateTime</returns>
        public static DateTime ToDateTime(this NSDate nsDate)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(_nsRef);
            var confertedDate = reference.AddSeconds(nsDate.SecondsSinceReferenceDate);
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(confertedDate))
            {
                confertedDate = confertedDate.AddHours(1);
            }
            
            // We loose granularity below millisecond range but that is probably ok
            return confertedDate;

        }
    }
}
