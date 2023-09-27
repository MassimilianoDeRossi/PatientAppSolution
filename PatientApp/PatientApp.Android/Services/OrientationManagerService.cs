using System.IO;
using Android.App;
using Android.Content.PM;
using Xamarin.Forms;

using PatientApp.Droid.Services;
using PatientApp.Services;

[assembly: Dependency (typeof (OrientationManagerService))]

namespace PatientApp.Droid.Services
{
	public class OrientationManagerService : IOrientationManager
	{
	    public void ForceLandscape()
	    {
            ((Activity)Forms.Context).RequestedOrientation = ScreenOrientation.Landscape;
        }

	    public void ForcePortrait()
	    {
            ((Activity)Forms.Context).RequestedOrientation = ScreenOrientation.Portrait;
        }

        public void DisableForcedOrientation()
        {
            ((Activity)Forms.Context).RequestedOrientation = ScreenOrientation.FullSensor;
        }
    }
}