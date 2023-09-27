using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Android.Media;
using Android.Graphics;
using System.Text.RegularExpressions;
using Plugin.CurrentActivity;
using System.Collections.Generic;
using System.Linq;
using Permission = Plugin.Permissions.Abstractions.Permission;
using Plugin.Permissions.Abstractions;

namespace PatientApp.Droid.Utilities
{
    public static class CameraWithOverlay
    {
        static readonly bool _isCameraAvailable;
        static readonly Context _context;
        static int requestId;
        static TaskCompletionSource<byte[]> completionSource;
        static IList<string> requestedPermissions;

        static CameraWithOverlay()
        {
            _context = Android.App.Application.Context;
            _isCameraAvailable = _context.PackageManager.HasSystemFeature(PackageManager.FeatureCamera);
        }

        public static async Task<byte[]> TakePicture(string overlayImagePath, float alpha)
        {
            if (!_isCameraAvailable)
                throw new NotSupportedException();

            if (!(await RequestCameraPermissions()))
            {
                throw new MediaPermissionException(Permission.Camera);
            }

            var media = await TakeMediaAsync(overlayImagePath, alpha);
            return media;
        }

        private static Task<byte[]> TakeMediaAsync(string overlayImagePath, float alpha)
        {
            int id = GetRequestId();

            var ntcs = new TaskCompletionSource<byte[]>(id);
            if (Interlocked.CompareExchange(ref completionSource, ntcs, null) != null)
                throw new InvalidOperationException("Only one operation can be active at a time");

            _context.StartActivity(CreateMediaIntent(id, overlayImagePath, alpha));

            EventHandler<MediaPickedEventArgs> handler = null;
            handler = (s, e) =>
            {
                var tcs = Interlocked.Exchange(ref completionSource, null);

                MediaPickerActivity.MediaPicked -= handler;

                if (e.RequestId != id)
                    return;

                if (e.IsCanceled)
                    tcs.SetResult(null);
                else if (e.Error != null)
                    tcs.SetException(e.Error);
                else
                    tcs.SetResult(e.Media);
            };

            MediaPickerActivity.MediaPicked += handler;

            return completionSource.Task;
        }

        static private int GetRequestId()
        {
            var id = requestId;
            if (requestId == int.MaxValue)
                requestId = 0;
            else
                requestId++;

            return id;
        }

        static private bool HasPermissionInManifest(string permission)
        {
            try
            {
                if (requestedPermissions != null)
                    return requestedPermissions.Any(r => r.Equals(permission, StringComparison.InvariantCultureIgnoreCase));

                if (_context == null)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to detect current Activity or App Context. Please ensure Plugin.CurrentActivity is installed in your Android project and your Application class is registering with Application.IActivityLifecycleCallbacks.");
                    return false;
                }

                var info = _context.PackageManager.GetPackageInfo(_context.PackageName, PackageInfoFlags.Permissions);

                if (info == null)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to get Package info, will not be able to determine permissions to request.");
                    return false;
                }

                requestedPermissions = info.RequestedPermissions;

                if (requestedPermissions == null)
                {
                    System.Diagnostics.Debug.WriteLine("There are no requested permissions, please check to ensure you have marked permissions you want to request.");
                    return false;
                }

                return requestedPermissions.Any(r => r.Equals(permission, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                Console.Write("Unable to check manifest for permission: " + ex);
            }
            return false;
        }

        static private async Task<bool> RequestCameraPermissions()
        {
            //We always have permission on anything lower than marshmallow.
            if ((int)Build.VERSION.SdkInt < 23)
                return true;

            bool checkCamera = HasPermissionInManifest(Android.Manifest.Permission.Camera);

            var hasStoragePermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            var hasCameraPermission = PermissionStatus.Granted;
            if (checkCamera)
                hasCameraPermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);


            var permissions = new List<Permission>();

            if (hasCameraPermission != PermissionStatus.Granted)
                permissions.Add(Permission.Camera);

            if (hasStoragePermission != PermissionStatus.Granted)
                permissions.Add(Permission.Storage);

            if (permissions.Count == 0) //good to go!
                return true;

            var results = await CrossPermissions.Current.RequestPermissionsAsync(permissions.ToArray());

            if (results.ContainsKey(Permission.Storage) &&
                    results[Permission.Storage] != PermissionStatus.Granted)
            {
                Console.WriteLine("Storage permission Denied.");
                return false;
            }

            if (results.ContainsKey(Permission.Camera) &&
                    results[Permission.Camera] != PermissionStatus.Granted)
            {
                Console.WriteLine("Camera permission Denied.");
                return false;
            }

            return true;
        }

        static private Intent CreateMediaIntent(int id, string overlayImagePath, float alpha)
        {
            Intent pickerIntent = new Intent(_context, typeof(MediaPickerActivity));
            pickerIntent.PutExtra(MediaPickerActivityIntent.EXTRA_ID, id);
            pickerIntent.PutExtra(MediaPickerActivityIntent.EXTRA_ALPHA, alpha);
            pickerIntent.PutExtra(MediaPickerActivityIntent.EXTRA_OVERLAY_IMAGE, overlayImagePath);
            pickerIntent.SetFlags(ActivityFlags.NewTask);
            return pickerIntent;
        }
    }
}