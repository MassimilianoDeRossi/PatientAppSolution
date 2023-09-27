using Xamarin.Forms;
using PatientApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using PatientApp.Droid.Utilities;
using Java.IO;

[assembly: Dependency(typeof(PatientApp.Droid.Services.TakePhotoWithOverlayImplementation))]

namespace PatientApp.Droid.Services
{
    public class TakePhotoWithOverlayImplementation : ITakePhotoWithOverlay
    {
        readonly string imagesDirectory;

        public TakePhotoWithOverlayImplementation()
        {
            imagesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Pictures");
            Directory.CreateDirectory(imagesDirectory); //if already exists, do nothing
        }

        public IEnumerable<string> GetFolders()
        {
            var folders = Directory.GetDirectories(imagesDirectory);

            var result = new List<string>();
            foreach (var folder in folders)
            {
                var folderName = Path.GetFileNameWithoutExtension(folder);
                if (!string.IsNullOrEmpty(folderName))
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
                return new string[0];
        }

        public async void TakePhoto(string folderName, string fileName, string overlayImagePath, float alpha, Action<string> successCallback, Action<string> cancelCallback)
        {
            string path = Path.Combine(imagesDirectory, folderName);
            if (!Directory.Exists(path))
            {
                cancelCallback.Invoke("Invalid path");
                return;
            }

            try
            {
                var mediaFileBytes = await CameraWithOverlay.TakePicture(overlayImagePath, alpha);

                if (mediaFileBytes == null || mediaFileBytes.Length == 0) //tapped back button
                {
                    return;
                }

                string jpgFileName = System.IO.Path.Combine(path, fileName);
                FileOutputStream fos = new FileOutputStream(jpgFileName);
                fos.Write(mediaFileBytes);
                fos.Close();
                successCallback?.Invoke(jpgFileName);
            }
            catch(Exception ex)
            {
                cancelCallback.Invoke(ex.Message);
            }
        }
    }
}
