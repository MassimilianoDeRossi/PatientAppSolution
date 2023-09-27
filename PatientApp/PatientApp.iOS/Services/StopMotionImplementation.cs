using Foundation;
using Xamarin.Forms;
using PatientApp.Services;
using UIKit;
using System;
using System.Collections.Generic;
using System.IO;
using PatientApp.iOS.StopMotion;
using MediaPlayer;

[assembly: Dependency(typeof(PatientApp.iOS.Services.StopMotionImplementation))]

namespace PatientApp.iOS.Services
{
    public class StopMotionImplementation : IStopMotion
    {
        string imagesDirectory, videoDirectory;
        public StopMotionImplementation()
        {
            imagesDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            videoDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public void StartBuildVideo(List<string> sourceImages, string folderName, string fileName, int frameDuration, Action<long, long> progressCallback, Action<string> completedCallback, Action<string> errorCallback)
        {
            if (sourceImages.Count == 0)
            {
                errorCallback("No images in source list");
                return;
            }

            string videoOutputPath = Path.Combine(videoDirectory, folderName, fileName);
            var builder = new StopMotionBuilder(sourceImages, videoOutputPath);
            builder.Build(progressCallback, completedCallback, errorCallback);
        }

        public IEnumerable<string> GetBuiltVideos(string folderName)
        {
            string path = Path.Combine(imagesDirectory, folderName);
            if (Directory.Exists(path))
                return Directory.GetFiles(path, "*.mp4");
            else
                return null;
        }

        public bool VideoFileExists(string fullPath)
        {
           return File.Exists(fullPath);
        }

        public void DeleteVideoFile(string fullPath)
        {
            File.Delete(fullPath);
        }

      
    }
}
