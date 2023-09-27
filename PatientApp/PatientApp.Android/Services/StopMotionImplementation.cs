using PatientApp.Droid.Utilities;
using PatientApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(PatientApp.Droid.Services.StopMotionImplementation))]
namespace PatientApp.Droid.Services
{
    public class StopMotionImplementation : IStopMotion
    {
        readonly string videoDirectory;

        public StopMotionImplementation()
        {
            videoDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Movies");
        }

        public void StartBuildVideo(List<string> sourceImages, string folderName, string fileName, int frameDuration, Action<long, long> progressCallback, Action<string> completedCallback, Action<string> errorCallback)
        {
            var videoOutputPath = Path.Combine(videoDirectory, folderName, fileName);
            Directory.CreateDirectory(Path.Combine(videoDirectory, folderName)); //if already exists, do nothing

            var builder = new StopMotionBuilder();
            var videoParams = new AndroidTimeLapseParameters()
            {
                PhotoUrls = sourceImages,
                VideoOutPath = videoOutputPath
            };
            builder.Build(videoParams, progressCallback, completedCallback, errorCallback);
        }

        public IEnumerable<string> GetBuiltVideos(string folderName)
        {
            string path = Path.Combine(videoDirectory, folderName);
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