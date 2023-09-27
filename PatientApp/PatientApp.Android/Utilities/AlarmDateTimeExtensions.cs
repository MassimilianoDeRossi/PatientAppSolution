using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PatientApp.Droid.Utilities
{
    public static class AlarmDateTimeExtensions
    {
        private static TimeSpan tsDiff = new DateTime(1970, 1, 1) - DateTime.MinValue;

        public static long ToAlarmMilliseconds(this DateTime dt)
        {
            // Convert the alarm time to UTC
            var utcAlarmTime = TimeZoneInfo.ConvertTimeToUtc(dt.ToLocalTime());

            // Work out the difference between epoch (Java) and ticks (.NET)
            var t = new DateTime(1970, 1, 1) - DateTime.MinValue;
            var epochDifferenceInSeconds = t.TotalSeconds;

            // Convert from ticks to milliseconds
            var utcAlarmTimeInMillis = utcAlarmTime.AddSeconds(-epochDifferenceInSeconds).Ticks / 10000;

            return utcAlarmTimeInMillis;
        }
    }
}