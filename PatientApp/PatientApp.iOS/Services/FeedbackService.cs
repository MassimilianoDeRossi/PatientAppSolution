using Xamarin.Forms;
using PatientApp.Services;

[assembly: Dependency (typeof (PatientApp.iOS.Services.FeedbackService))]
namespace PatientApp.iOS.Services
{
    public class FeedbackService : IFeedbackService
    {
        public void GetFeedback()
        {
            // TODO: REPLACE WITH NEW IMPLEMENTATION
            //BITHockeyManager.SharedHockeyManager.FeedbackManager.ShowFeedbackListView();
        }
        
    }
}
