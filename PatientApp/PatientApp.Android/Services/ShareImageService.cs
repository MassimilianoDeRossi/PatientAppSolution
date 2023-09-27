using System;
using System.Collections.Generic;
using PatientApp.Services;
using Android.Graphics;
using Android.Content;
using Java.IO;
using Android.App;
using Xamarin.Forms;
using System.Linq;
using Android.Support.V4.Content;

[assembly: Dependency(typeof(PatientApp.Droid.Services.ShareImageServiceImplementation))]
namespace PatientApp.Droid.Services
{
    public class ShareImageServiceImplementation : IShareMediaService
    {
        public void ShareImage(string imageFilePath)
        {
            Intent share = new Intent(Intent.ActionSend);
            share.SetFlags(ActivityFlags.NewTask);
            share.SetType("image/jpeg");
            File filelocation = new File(imageFilePath); //to fix email attachment
            share.PutExtra(Intent.ExtraStream, FileProvider.GetUriForFile((Activity)Forms.Context, "com.orthofix.myhexplan.on.fileprovider", filelocation));
            ((Activity)Forms.Context).StartActivity(Intent.CreateChooser(share, "Share Image"));
        }

        public void ShareImages(IEnumerable<string> imageFilePaths, Action successCallback)
        {
            throw new NotImplementedException();
        }

        public void ShareImagesAndVideos(IEnumerable<string> imageFilePaths, IEnumerable<string> videoFilePaths, Action successCallback)
        {
            Intent share = new Intent(Intent.ActionSendMultiple);
            share.SetFlags(ActivityFlags.NewTask);
            share.SetType("*/*");
            var context = (Activity)Forms.Context;

            var files = new List<Android.OS.IParcelable>();

            foreach (var fileCompletePath in imageFilePaths)
            {
                files.Add(FileProvider.GetUriForFile(context, "com.orthofix.myhexplan.on.fileprovider", new File(fileCompletePath)));
            }
            foreach (var fileCompletePath in videoFilePaths)
            {
                files.Add(FileProvider.GetUriForFile(context, "com.orthofix.myhexplan.on.fileprovider", new File(fileCompletePath)));
            }

            share.PutParcelableArrayListExtra(Intent.ExtraStream, files);
            context.StartActivity(Intent.CreateChooser(share, "Share Media"));
        }
    }
}