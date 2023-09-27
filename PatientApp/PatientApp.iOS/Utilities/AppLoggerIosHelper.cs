using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace PatientApp.iOS.Utilities
{  
    public class AppLoggerIosHelper : PatientApp.Utilities.AppLoggerFormHelper
    {
        static readonly LogLevel _logLevel;
        static readonly string _appId;
       
        static AppLoggerIosHelper() 
        {
            _exceptionEventName = "AppException";
            try
            {
                _logLevel = (LogLevel)int.Parse(PCLAppConfig.ConfigurationManager.AppSettings["LogLevel"]);
            }
            catch
            {
                _logLevel = LogLevel.Debug;
            }
            _appId = PCLAppConfig.ConfigurationManager.AppSettings["AppLoggerId"];

        }
        public static void Init()
        {
            // TODO: Check if needed 
            ////Disable username and email in feedbackform.
            //BITHockeyManager.SharedHockeyManager.FeedbackManager.RequireUserEmail = BITFeedbackUserDataElement.DontShow;
            //BITHockeyManager.SharedHockeyManager.FeedbackManager.RequireUserName = BITFeedbackUserDataElement.DontShow;
            AppCenter.Start(_appId, typeof(Analytics), typeof(Crashes));
        }

    }

}