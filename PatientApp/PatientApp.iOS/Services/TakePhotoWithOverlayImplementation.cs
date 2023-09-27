using Foundation;
using Xamarin.Forms;
using PatientApp.Services;
using UIKit;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using PatientApp.iOS.StopMotion;
using PatientApp.iOS.Camera;
using Photos;

[assembly: Dependency(typeof(PatientApp.iOS.Services.TakePhotoWithOverlayImplementation))]

namespace PatientApp.iOS.Services
{
    public class TakePhotoWithOverlayImplementation : ITakePhotoWithOverlay
    {
        string imagesDirectory;

        public TakePhotoWithOverlayImplementation()
        {
            imagesDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        }

        public IEnumerable<string> GetFolders()
        {
            var folders = Directory.GetDirectories(imagesDirectory);

            var result = new List<string>();
            foreach (var folder in folders)
            {
                var folderName = Path.GetFileNameWithoutExtension(folder);
                
                // Temp folder in reserved by CrossMedia plugin (pickPhotoAsync method)
                if (!string.IsNullOrEmpty(folderName) && !folderName.Equals("temp", StringComparison.InvariantCultureIgnoreCase))
                    result.Add(folderName);
            }

            return result;

        }

        public void CreateFolder(string folderName)
        {
            string path = Path.Combine(imagesDirectory, folderName);
            Directory.CreateDirectory(path);
        }

        public void DeleteFolder(string folderName)
        {
            string path = Path.Combine(imagesDirectory, folderName);
            Directory.Delete(path, true);            
        }

        public void DeletePhoto(string imagePath)
        {
            System.IO.File.Delete(imagePath);
        }

        public IEnumerable<string> GetTakenPhotoes(string folderName)
        {
            string path = Path.Combine(imagesDirectory, folderName);
            if (Directory.Exists(path))
                return Directory.GetFiles(path, "*.jpg");
            else
                return null;
        }

        public void TakePhoto(string folderName, string fileName, string overlayImagePath, float alpha, Action<string> successCallback, Action<string> cancelCallback)
        {
            string path = Path.Combine(imagesDirectory, folderName);
            if (!Directory.Exists(path))
            {
                cancelCallback.Invoke("Invalid path");
                return;
            }

            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;

            var result = CameraWithOverlay.TakePicture(vc, overlayImagePath, (obj) =>
            {
                if (obj != null)
                {
                    // https://developer.apple.com/library/ios/#documentation/uikit/reference/UIImagePickerControllerDelegate_Protocol/UIImagePickerControllerDelegate/UIImagePickerControllerDelegate.html#//apple_ref/occ/intfm/UIImagePickerControllerDelegate/imagePickerController:didFinishPickingMediaWithInfo:
                    var photo = obj.ValueForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;
                    var meta = obj.ValueForKey(new NSString("UIImagePickerControllerMediaMetadata")) as NSDictionary;

                    string jpgFileName = System.IO.Path.Combine(path, fileName);
                    NSData imgData = photo.AsJPEG();
                    NSError err = null;
                    if (imgData.Save(jpgFileName, false, out err))
                    {
                        successCallback?.Invoke(jpgFileName);
                    }
                    else
                    {
                        cancelCallback.Invoke(err.LocalizedDescription);
                    }
                }
            });

            if (!result)
                cancelCallback("No camera available");
        }
    }
}
