
using System;
using UIKit;
using Foundation;
using System.Collections.Generic;

namespace PatientApp.iOS.Camera
{
    public static class CameraWithOverlay
    {
        static UIImagePickerController picker;
        static UIImageView overlayView;
        static Action<NSDictionary> _callback;

        static void Init()
        {
            if (picker != null)
                return;

            picker = new UIImagePickerController();
            picker.Delegate = new CameraDelegate();

            // Subscribe preview show / hide events
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("_UIImagePickerControllerUserDidCaptureItem"),
              (message) =>
              {
                  // Hide overlay during preview
                  overlayView.Hidden = true;

              },
              null);

            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("_UIImagePickerControllerUserDidRejectItem"),
              (message) =>
              {
                  overlayView.Hidden = false;
              },
              null);
        }

        class CameraDelegate : UIImagePickerControllerDelegate
        {
            public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
            {
                var cb = _callback;
                _callback = null;

                picker.DismissModalViewController(true);
                cb(info);
            }

            public override void Canceled(UIImagePickerController picker)
            {
                // Should not call base method
                //base.Canceled(picker);
                picker.DismissModalViewController(true);
            }
        }

        public static bool TakePicture(UIViewController parent, string overlayImagePath, Action<NSDictionary> callback)
        {
            if (!UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
            {
                callback(null);
                return false;
            }                

            Init();            
            
            picker.SourceType = UIImagePickerControllerSourceType.Camera;
            picker.ShowsCameraControls = true;
            picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
            // Disable editing on preview
            picker.AllowsEditing = false;

            // create the overlay view
            overlayView = new UIImageView(); //  new CoreGraphics.CGRect(0, 0, parent.View.Frame.Size.Width, parent.View.Frame.Size.Height));
                                             // important - it needs to be transparent so the camera preview shows through!
            overlayView.Opaque = false;
            overlayView.Alpha = 0.6f;
            overlayView.BackgroundColor = UIColor.Clear;
            overlayView.UserInteractionEnabled = false;

            if (!string.IsNullOrEmpty(overlayImagePath) && System.IO.File.Exists(overlayImagePath))
            {
                var image = new UIImage(overlayImagePath);

                //var width = parent.View.Frame.Size.Width;
                //var ratio = image.Size.Height / image.Size.Width;
                //var height = width * ratio;
                //overlayView.Frame = new CoreGraphics.CGRect(0, 0, width, height);
                overlayView.Image = image;
                overlayView.ContentMode = UIViewContentMode.ScaleAspectFit;
                overlayView.Frame = new CoreGraphics.CGRect(0, 43, parent.View.Frame.Size.Width, parent.View.Frame.Size.Height - 160);
            }
            else
            {
                overlayView.Frame = new CoreGraphics.CGRect(0, 43, parent.View.Frame.Size.Width, parent.View.Frame.Size.Height - 160);
            }

            picker.CameraOverlayView = overlayView;

            _callback = callback;
            try
            {
                parent.PresentModalViewController(picker, true);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public static void SelectPicture(UIViewController parent, Action<NSDictionary> callback)
        {
            Init();
            picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            _callback = callback;
            parent.PresentModalViewController(picker, true);
        }
    }
}
