using System;
using System.Collections.Generic;
using Foundation;
using Xamarin.Forms;
using UIKit;
using PatientApp.Services;
using AssetsLibrary;

[assembly: Dependency (typeof (PatientApp.iOS.Services.ShareMediaServiceImplementation))]

namespace PatientApp.iOS.Services
{
	public class ShareMediaServiceImplementation : IShareMediaService
	{
		public ShareMediaServiceImplementation()
		{
		}

	    public void ShareImage(string fileName)
	    {
            var imageUrl = NSUrl.FromFilename(fileName);

            var activityItems = new[] { imageUrl.Copy() };

            var activityController = new UIActivityViewController(activityItems, null);

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            topController.PresentViewController(activityController, true, () => { });
        }

        public void ShareImages(IEnumerable<string> fileNames, Action successCallback)
        {
            var activityItems = new List<NSObject>();

            foreach (var fileName in fileNames)
            {
                var imageUrl = NSUrl.FromFilename(fileName);
                activityItems.Add(imageUrl.Copy());                
            }

            var activityController = new UIActivityViewController(activityItems.ToArray(), null);

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            topController.PresentViewController(activityController, true, successCallback);
        }

        public void ShareImagesAndVideos(IEnumerable<string> imageFilePaths, IEnumerable<string> videoFilePaths, Action successCallback)
        {
            var activityItems = new List<NSObject>();

            foreach (var fileName in imageFilePaths)
            {
                var image = new UIImage(fileName);
                activityItems.Add(image);
            }

            foreach (var fileName in videoFilePaths)
            {
                var videoUrl = NSUrl.FromFilename(fileName);
                activityItems.Add(videoUrl.Copy());
            }

            var activityController = new UIActivityViewController(activityItems.ToArray(), null);

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            topController.PresentViewController(activityController, true, successCallback);
        }
    }
}
