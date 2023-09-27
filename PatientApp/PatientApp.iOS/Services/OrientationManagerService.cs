using System;
using System.IO;
using Foundation;
using Xamarin.Forms;
using PatientApp.Services;
using UIKit;

[assembly: Dependency (typeof (PatientApp.iOS.Services.OrientationManagerService))]

namespace PatientApp.iOS.Services
{
	public class OrientationManagerService : IOrientationManager
	{
        public void ForceLandscape()
	    {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));
        }

	    public void ForcePortrait()
	    {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
        }
        public void DisableForcedOrientation()
        {
            // Do nothing

        }

    }
}
