using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using System.Threading.Tasks;
using Android.Widget;
using System.Collections.Generic;
using System.IO;
using Android.Hardware;

namespace PatientApp.Droid
{
    /// <summary>
    /// Intent parameters name for media picker activity
    /// </summary>
    public static class MediaPickerActivityIntent
    {
        public const string EXTRA_ID = "id";
        public const string EXTRA_ALPHA = "alpha";
        public const string EXTRA_OVERLAY_IMAGE = "overlayImage";
    }

    /// <summary>
    /// Activity that take a picture using Camera Api (deprecated). Use MediaPickerActivityCamera2Api instead.
    /// </summary>
    [Activity(Label = "Graphics/CameraOverlay", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MediaPickerActivity : Activity, ISurfaceHolderCallback, Camera.IPictureCallback
    {
        ISurfaceHolder _holder, _holderTransparent;
        Camera _camera;
        Context _context;
        SurfaceView _cameraView;
        int _cameraId;

        internal static event EventHandler<MediaPickedEventArgs> MediaPicked;

        private int id;
        private float alpha;
        private string overlayimage;
        private string overlayimageWithExt;

        private int Rotation
        {
            get
            {
                return _cameraId == (int)CameraFacing.Back ? 90 : -90;
            }
        }

        public override void OnBackPressed()
        {
            var e = new MediaPickedEventArgs(id, true);
            Finish();
            System.Threading.Thread.Sleep(50);
            OnMediaPicked(e);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt(MediaPickerActivityIntent.EXTRA_ID, id);
            outState.PutFloat(MediaPickerActivityIntent.EXTRA_ALPHA, alpha);
            outState.PutString(MediaPickerActivityIntent.EXTRA_OVERLAY_IMAGE, overlayimageWithExt);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var intent = (savedInstanceState ?? Intent.Extras);
            id = intent.GetInt(MediaPickerActivityIntent.EXTRA_ID, 0);
            alpha = intent.GetFloat(MediaPickerActivityIntent.EXTRA_ALPHA);
            overlayimageWithExt = intent.GetString(MediaPickerActivityIntent.EXTRA_OVERLAY_IMAGE);
            overlayimage = Path.GetFileNameWithoutExtension(overlayimageWithExt);
            _cameraId = (int)CameraFacing.Back;

            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.TransparentCamera);
            _cameraView = (SurfaceView)FindViewById(Resource.Id.CameraView);
            _holder = _cameraView.Holder;
            _holder.AddCallback(this);
            _holder.SetType(SurfaceType.PushBuffers);
            _cameraView.SetSecure(true);

            // Create second surface with another holder (holderTransparent)
            var transparentView = (SurfaceView)FindViewById(Resource.Id.TransparentView);
            _holderTransparent = transparentView.Holder;
            _holderTransparent.AddCallback(this);
            _holderTransparent.SetFormat(Android.Graphics.Format.Translucent);
            transparentView.SetZOrderMediaOverlay(true);

            _context = Application.Context;

            ImageButton captureButton = (ImageButton)FindViewById(Resource.Id.button_capture);
            captureButton.Click += delegate
            {
                try
                {
                    _camera.TakePicture(null, null, this);
                }
                catch (Exception ex)
                {
                    var e = new MediaPickedEventArgs(id, ex);
                    Finish();
                    System.Threading.Thread.Sleep(50);
                    OnMediaPicked(e);
                }
            };

            ImageButton switchButton = (ImageButton)FindViewById(Resource.Id.switch_camera);
            switchButton.Click += delegate
            {
                ChangeCamera();
            };
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (holder == _holder) //cameraholder
            {
                if (_camera == null && !CreateCamera())
                {
                    System.Console.WriteLine("Camera initialization failed.");
                }
            }
            else if (holder == _holderTransparent)
            {
                DrawOverlay();
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            if (_camera != null)
            {
                _camera.Release();
                _camera = null;
            }
        }

        public void ChangeCamera()
        {
            _camera.Release();
            _camera = null;

            if (_cameraId == (int)CameraFacing.Front)
            {
                _cameraId = (int)CameraFacing.Back;
            }
            else
            {
                _cameraId = (int)CameraFacing.Front;
            }

            if (CreateCamera())
            {
                RefreshCamera();
            }
        }

        public bool CreateCamera()
        {
            try
            {
                _camera = Camera.Open(_cameraId);

                var param = _camera.GetParameters();
                param.JpegQuality = 100;
                if (param.SupportedFocusModes.Contains("continuous-picture"))
                {
                    param.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
                }

                var PictureSizes = param.SupportedPictureSizes;
                var PictureSize = new Camera.Size(_camera, 0, 0);
                for (int i = 0; i < PictureSizes.Count; i++)
                {
                    if (PictureSizes[i].Width > PictureSize.Width && PictureSizes[i].Width <= 1920 && PictureSizes[i].Height <= 1080) //to avoid problem with out of memory
                        PictureSize = PictureSizes[i];
                }

                //add control with device aspect ratio
                param.SetPictureSize(PictureSize.Width, PictureSize.Height);
                _camera.SetParameters(param);

                var PreviewSizes = param.SupportedPreviewSizes;
                var previewSize = GetOptimalPreviewSize(PreviewSizes, App.ScreenWidth, App.ScreenHeight);
                param.SetPreviewSize(previewSize.Width, previewSize.Height);
                _camera.SetParameters(param);
                _camera.SetDisplayOrientation(90);
                _camera.StartPreview();
                return true;
            }
            catch
            {
                var e = new MediaPickedEventArgs(id, new Exception("Cannot open camera. It's probably already open."));
                Finish();
                System.Threading.Thread.Sleep(50);
                OnMediaPicked(e);
                return false;
            }
        }

        public void RefreshCamera()
        {
            if (_holder.Surface == null || _camera == null)
            {
                return;
            }
            try
            {
                _camera.StopPreview();
            }
            finally
            {
                _camera.SetPreviewDisplay(_holder);
                _camera.StartPreview();
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int w, int h)
        {
            RefreshCamera();
        }

        private async void DrawOverlay()
        {
            var canvas = _holderTransparent.LockCanvas(null);
            if (canvas != null)
            {
                var paint = new Android.Graphics.Paint();
                paint.Alpha = (int)(alpha * 255);

                int iconId = _context.Resources.GetIdentifier(overlayimage, "drawable", _context.PackageName);
                Android.Graphics.Bitmap image = await Android.Graphics.BitmapFactory.DecodeResourceAsync(_context.Resources, iconId);
                if (image != null)
                {
                    int height = _cameraView.Height;
                    float aspectRatioResize = (float)_cameraView.Height / image.Height;
                    int width = (int)(image.Width * aspectRatioResize);
                    int horizontalPosition = _cameraView.Width / 2 - width / 2;
                    canvas.DrawBitmap(image, null, new Android.Graphics.Rect(horizontalPosition, 0, horizontalPosition + width, height), paint);
                }

                _holderTransparent.UnlockCanvasAndPost(canvas);
            }
        }

        public void OnPictureTaken(byte[] data, Camera camera)
        {
            try //sometimes it goes out of memory
            {
                Android.Graphics.Bitmap realImage = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
                var imageRotated = Rotate(realImage, Rotation);
                byte[] imgRotatedByte;

                using (var stream = new MemoryStream())
                {
                    imageRotated.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);
                    imgRotatedByte = stream.ToArray();
                }

                var e = new MediaPickedEventArgs(id, false, imgRotatedByte);
                Finish();
                System.Threading.Thread.Sleep(50);
                OnMediaPicked(e);
            }
            catch (Exception ex)
            {
                var e = new MediaPickedEventArgs(id, ex);
                Finish();
                System.Threading.Thread.Sleep(50);
                OnMediaPicked(e);
            }
        }

        private Android.Graphics.Bitmap Rotate(Android.Graphics.Bitmap bitmap, int degree)
        {
            Android.Graphics.Matrix mtx = new Android.Graphics.Matrix();
            mtx.SetRotate(degree);

            return Android.Graphics.Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mtx, true);
        }

        private static void OnMediaPicked(MediaPickedEventArgs e) => MediaPicked?.Invoke(null, e);

        private Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h)
        {
            double ASPECT_TOLERANCE = 0.1;

            Camera.Size optimalSize = null;

            var targetRatio = (double)h / (double)w;

            if (sizes == null) return null;

            double minDiff = Double.MaxValue;

            int targetHeight = h;

            foreach (var size in sizes)
            {
                double ratio = (double)size.Width / size.Height;
                if (Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE) continue;
                if (Math.Abs(size.Height - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            if (optimalSize == null)
            {
                minDiff = Double.MaxValue;
                foreach (Camera.Size size in sizes)
                {
                    if (Math.Abs(size.Height - targetHeight) < minDiff)
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }
            return optimalSize;

        }


    }


    internal class MediaPickedEventArgs : EventArgs
    {
        public MediaPickedEventArgs(int id, Exception error)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            RequestId = id;
            Error = error;
        }

        public MediaPickedEventArgs(int id, bool isCanceled, byte[] media = null)
        {
            RequestId = id;
            IsCanceled = isCanceled;
            if (!IsCanceled && media == null)
                throw new ArgumentNullException("media");

            Media = media;
        }

        public int RequestId
        {
            get;
        }

        public bool IsCanceled
        {
            get;
        }

        public Exception Error
        {
            get;
        }

        public byte[] Media
        {
            get;
        }

        public Task<byte[]> ToTask()
        {
            var tcs = new TaskCompletionSource<byte[]>();

            if (IsCanceled)
                tcs.SetResult(null);
            else if (Error != null)
                tcs.SetException(Error);
            else
                tcs.SetResult(Media);
            {
                return tcs.Task;
            }
        }
    }
}