using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace PatientApp.Utilities
{
    /// <summary>
    /// App Exceptions and events logger 
    /// </summary>
    public class AppLoggerHelper
    {
        private static bool _initialized = false;
        protected static string _exceptionEventName = "AppException";

        /// <summary>
        /// Log an exception 
        /// </summary>
        /// <param name="ex">the exception to log</param>
        /// <param name="message">context messages</param>
        /// <param name="traceLevel">the error severity (optional)</param>
        public static void LogException(Exception ex, string message, TraceLevel traceLevel = TraceLevel.Warning)
        {
            var arguments = CreateArgumentList(message + Environment.NewLine + ex.ToString(), traceLevel);
            //Analytics.TrackEvent(_exceptionEventName, arguments);
            Crashes.TrackError(ex, arguments);            
        }


        /// <summary>
        /// Log an event 
        /// </summary>
        /// <param name="eventName">Event category </param>
        /// <param name="message">message related to category (if it matters to specify additional infos)</param>
        /// <param name="traceLevel">The message severity</param>
        public static void LogEvent(string eventName, string message, TraceLevel traceLevel = TraceLevel.Info)
        {
            var arguments = CreateArgumentList(message, traceLevel);
            Analytics.TrackEvent(eventName, arguments);
        }

        /// <summary>
        /// Initialize App exceptions and events Logger 
        /// </summary>
        public static void Init()
        {
            if (_initialized)
                return;

            var androidAppId = PCLAppConfig.ConfigurationManager.AppSettings["AppLoggerAndroidId"];
            var iosAppId = PCLAppConfig.ConfigurationManager.AppSettings["AppLoggerIosId"];
            var logLevel = LogLevel.Debug;
            try
            {
                logLevel = (LogLevel)int.Parse(PCLAppConfig.ConfigurationManager.AppSettings["LogLevel"]);
            }
            catch
            {
            }
            AppCenter.Start(string.Format("ios={0};android={1}", iosAppId, androidAppId), typeof(Analytics), typeof(Crashes));
            AppCenter.LogLevel = logLevel;

            _initialized = true;
        }

        private static Dictionary<string, string> CreateArgumentList(string message, TraceLevel severity)
        {
            var arguments = new Dictionary<string, string>();
            arguments.Add("user_Id", App.PushNotificationToken ?? "No token");
            if (Settings.AppSettings.Instance != null)
                arguments.Add("app_instance_Id", Settings.AppSettings.Instance.ApplicationInstanceId.ToString());
            arguments.Add("message", message);
            arguments.Add("severity", severity.ToString());
            return arguments;
        }
    }

}