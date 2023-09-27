using Xamarin.Forms;
using PatientApp.Droid.Services;
using Android.App;
using PatientApp.Services;
using PatientApp.Droid.Utilities;

[assembly: Dependency(typeof(FeedbackService))]
namespace PatientApp.Droid.Services
{
    public class FeedbackService : IFeedbackService
    {
        public void GetFeedback()
        {
            // TODO: REPLACE WITH A NEW IMPLEMENTATION
            //FeedbackManager.RequireUserEmail = FeedbackUserDataElement.DontShow;
            //FeedbackManager.RequireUserName = FeedbackUserDataElement.DontShow;
            //FeedbackManager.ShowFeedbackActivity(AppLoggerAndroidHelper.Context);
        }
    }
}