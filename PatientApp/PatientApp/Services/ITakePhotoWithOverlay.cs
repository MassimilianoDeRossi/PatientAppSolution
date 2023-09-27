using System;
using System.Collections.Generic;

namespace PatientApp.Services
{
    /// <summary>
    /// Dependency Service used to take photoes with an overlay image, manage albums and contained image files
    /// </summary>
    public interface ITakePhotoWithOverlay
    {
        /// <summary>
        /// Get list of folders (album) in the local storage
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetFolders();

        /// <summary>
        /// Create a new folder (album) in the local storage
        /// </summary>
        /// <param name="folderName"></param>
        void CreateFolder(string folderName);

        /// <summary>
        /// Delete a folder (and its content) from the local storage
        /// </summary>
        /// <param name="folderName"></param>
        void DeleteFolder(string folderName);

        /// <summary>
        /// Get the image files contained in a specific folder (album)
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        IEnumerable<string> GetTakenPhotoes(string folderName);

        /// <summary>
        /// Open device camera to take a photo showing an overlayed transparent image
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <param name="overlayImagePath"></param>
        /// <param name="alpha"></param>
        /// <param name="successCallback"></param>
        /// <param name="cancelCallback"></param>
        void TakePhoto(string folderName, string fileName, string overlayImagePath, float alpha, Action<string> successCallback, Action<string> cancelCallback);

        /// <summary>
        /// Delete an image file from local storage
        /// </summary>
        /// <param name="imagePath"></param>
        void DeletePhoto(string imagePath);
    }
}
