using System;
using System.Collections.Generic;

namespace PatientApp.Services
{
    /// <summary>
    /// Dependency Service used to implement native StopMotion features
    /// </summary>
    public interface IStopMotion
    {
        /// <summary>
        /// Build and locally save a video from provided source images. User callbacks to check results
        /// </summary>
        /// <param name="sourceImages"></param>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <param name="frameDuration"></param>
        /// <param name="progressCallback"></param>
        /// <param name="completedCallback"></param>
        /// <param name="errorCallback"></param>
        void StartBuildVideo(List<string> sourceImages, string folderName, string fileName, int frameDuration, Action<long, long> progressCallback, Action<string> completedCallback, Action<string> errorCallback);

        /// <summary>
        /// Get a list for previously saved video in a given folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        IEnumerable<string> GetBuiltVideos(string folderName);

        /// <summary>
        /// Check if a videofile exists in local storage
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        bool VideoFileExists(string fullPath);

        /// <summary>
        /// Delete a previously saved video file from local storage
        /// </summary>
        /// <param name="fullPath"></param>
        void DeleteVideoFile(string fullPath);
    }
}
