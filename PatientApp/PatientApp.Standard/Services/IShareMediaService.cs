using System;
using System.Collections.Generic;

namespace PatientApp.Services
{
    /// <summary>
    /// Dependency services used to implement native images and video Sharing 
    /// </summary>
    public interface IShareMediaService
    {
        /// <summary>
        /// Share a single images stored locally
        /// </summary>
        /// <param name="imageFilePath"></param>
        void ShareImage(string imageFilePath);

        /// <summary>
        /// Share a list of locally saved images and call a callback on success
        /// </summary>
        /// <param name="imageFilePaths"></param>
        /// <param name="successCallback"></param>
        void ShareImages(IEnumerable<string> imageFilePaths, Action successCallback);

        /// <summary>
        /// Share a list of images and a list of videos (stored locally). Call a callback on success
        /// </summary>
        /// <param name="imageFilePaths"></param>
        /// <param name="videoFilePaths"></param>
        /// <param name="successCallback"></param>
        void ShareImagesAndVideos(IEnumerable<string> imageFilePaths, IEnumerable<string> videoFilePaths, Action successCallback);
    }
}
