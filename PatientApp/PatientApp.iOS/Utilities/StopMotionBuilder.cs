using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using UIKit;

namespace PatientApp.iOS.StopMotion
{
    public class StopMotionBuilder
    {
        private const string kErrorDomain = "TimeLapseBuilder";
        private const int zFailedToStartAssetWriterError = 0;
        private const int kFailedToAppendPixelBufferError = 1;
        private const int FPS = 30;
        private const int frameDurationInFPS = FPS;
        private const int OUTPUT_WIDTH = 800;

        private List<string> _photoUrls;
        private string _videoOutputPath;
        private AVAssetWriter _videoWriter = null;

        public StopMotionBuilder(List<string> photoUrls, string videoPath)
        {
            _photoUrls = photoUrls;
            _videoOutputPath = videoPath;
        }

        public void Build(Action<long, long> progressCallback, Action<string> successCallback, Action<string> failureCallback)
        {
            var testImage = new UIImage(_photoUrls.First());

            var inputSize = new CGSize(width: testImage.Size.Width, height: testImage.Size.Height);

            // Scale output video 
            var outputWidth = Math.Min(OUTPUT_WIDTH, testImage.Size.Width);
            var outputHeight = (outputWidth / testImage.Size.Width) * testImage.Size.Height;

            var outputSize = new CGSize(width: outputWidth, height: outputHeight);

            NSError error;

            if (File.Exists(_videoOutputPath))
            {
                try
                {
                    File.Delete(_videoOutputPath);
                }
                catch
                {
                }
            }

            try
            {
                error = new NSError();
                var url = NSUrl.FromFilename(_videoOutputPath);
                //_videoWriter = new AVAssetWriter(url, AVFileType.QuickTimeMovie, out error);
                _videoWriter = new AVAssetWriter(url, AVFileType.Mpeg4, out error);
            }
            catch (Exception ex)
            {
            }

            if (_videoWriter != null)
            {
                var settings = new AVVideoSettingsCompressed()
                {
                    Codec = AVVideoCodec.H264,
                    Width = (int)outputSize.Width,
                    Height = (int)outputSize.Height,
                };
                var videoWriterInput = new AVAssetWriterInput(mediaType: AVMediaType.Video, outputSettings: settings);

                var sourceBufferAttributes = new NSMutableDictionary<NSString, NSObject>();
                sourceBufferAttributes.Add(CoreVideo.CVPixelBuffer.PixelFormatTypeKey, (NSNumber)(int)CoreVideo.CVPixelFormatType.CV32ARGB);
                sourceBufferAttributes.Add(CoreVideo.CVPixelBuffer.WidthKey, (NSNumber)(float)inputSize.Width);
                sourceBufferAttributes.Add(CoreVideo.CVPixelBuffer.HeightKey, (NSNumber)(float)inputSize.Height);

                var pixelBufferAdaptor = new AVAssetWriterInputPixelBufferAdaptor(videoWriterInput, sourcePixelBufferAttributes: sourceBufferAttributes);

                _videoWriter.AddInput(videoWriterInput);

                if (_videoWriter.StartWriting())
                {
                    _videoWriter.StartSessionAtSourceTime(CoreMedia.CMTime.Zero);
                    var mediaQueue = new DispatchQueue("mediaInputQueue");
                    videoWriterInput.RequestMediaData(mediaQueue, () =>
                    {
                        var frameDuration = new CMTime(frameDurationInFPS, FPS);

                        long frameCount = 0;
                        var remainingPhotoUrls = new List<string>(_photoUrls);

                        while (videoWriterInput.ReadyForMoreMediaData && remainingPhotoUrls.Any())
                        {
                            var nextPhotoUrl = remainingPhotoUrls[0];
                            remainingPhotoUrls.RemoveAt(0);
                            var lastFrameTime = new CMTime(frameCount * frameDurationInFPS, FPS);
                            //var presentationTime = frameCount == 0 ? lastFrameTime : CMTime.Add(lastFrameTime, frameDuration);
                            var presentationTime = lastFrameTime;

                            if (!appendPixelBufferForImageAtURL(nextPhotoUrl, pixelBufferAdaptor, presentationTime))
                            {
                                error = new NSError(domain: new NSString(kErrorDomain), code: kFailedToAppendPixelBufferError, userInfo: new NSDictionary());
                                break;
                            }

                            frameCount += 1;
                            progressCallback?.Invoke(frameCount, _photoUrls.Count);
                        }

                        videoWriterInput.MarkAsFinished();
                        _videoWriter.FinishWriting(() =>
              {
                  successCallback?.Invoke(_videoOutputPath);
              });
                    });
                }
            }

        }

        private bool appendPixelBufferForImageAtURL(string imageUrl, AVAssetWriterInputPixelBufferAdaptor adoptor, CMTime presentationTime)
        {
            bool appendSucceded = false;

            var image = new UIImage(imageUrl);

            var pixelBufferPool = adoptor.PixelBufferPool;

            CVPixelBuffer pixelBuffer = pixelBufferPool.CreatePixelBuffer();
            var fixedImage = FixImageRotation(image);

            //fillPixelBufferFromImage(image, pixelBuffer);
            fillPixelBufferFromImage(fixedImage, pixelBuffer);
            appendSucceded = adoptor.AppendPixelBufferWithPresentationTime(pixelBuffer, presentationTime);


            return appendSucceded;
        }

        private UIImage FixImageRotationOLD(UIImage image)
        {
            bool flip = false; //used to see if the image is mirrored
            bool isRotatedBy90 = false; // used to check whether aspect ratio is to be changed or not

            var transform = CGAffineTransform.MakeIdentity();
            switch (image.Orientation)
            {
                case UIImageOrientation.Down:
                case UIImageOrientation.DownMirrored:
                    transform.Rotate(new nfloat(System.Math.PI));
                    break;
                case UIImageOrientation.Left:
                case UIImageOrientation.LeftMirrored:
                    transform.Rotate(new nfloat(System.Math.PI / 2.0));
                    isRotatedBy90 = true;
                    break;
                case UIImageOrientation.Right:
                case UIImageOrientation.RightMirrored:
                    transform.Rotate(new nfloat(-System.Math.PI / 2.0));
                    isRotatedBy90 = true;
                    break;
                case UIImageOrientation.Up:
                case UIImageOrientation.UpMirrored:
                    break;

            }

            switch (image.Orientation)
            {
                case UIImageOrientation.UpMirrored:
                case UIImageOrientation.DownMirrored:
                    transform.Translate(image.Size.Width, 0);
                    flip = true;
                    break;
                case UIImageOrientation.LeftMirrored:
                case UIImageOrientation.RightMirrored:
                    transform.Translate(image.Size.Height, 0);
                    flip = true;
                    break;
            }

            var rect = new CGRect(new CGPoint(x: 0, y: 0), image.Size);
            UIView rotatedViewBox = null;
            CGSize rotatedSize = CGSize.Empty;

            image.InvokeOnMainThread(() =>
            {
                rotatedViewBox = new UIView(rect);
                rotatedViewBox.Transform = transform;
                rotatedSize = rotatedViewBox.Frame.Size;
            });

            UIGraphics.BeginImageContext(rotatedSize);

            using (var context = UIGraphics.GetCurrentContext())
            {
                //context.DrawImage(new CGRect(0, 0, width, height), image.CGImage);

                var tx = (nfloat)(rotatedSize.Width / 2.0);
                var ty = (nfloat)(rotatedSize.Height / 2.0);
                context.TranslateCTM(tx, ty);

                nfloat yFlip;

                if (flip)
                {
                    yFlip = -1.0f;
                }
                else
                {
                    yFlip = 1.0f;
                }

                context.ScaleCTM(yFlip, -1.0f);

                //check if we have to fix the aspect ratio
                if (isRotatedBy90)
                {
                    //bitmap?.draw(self.cgImage!, in: CGRect(x: -size.width / 2, y: -size.height / 2, width: size.height, height: size.width))
                    context.DrawImage(new CGRect(-image.Size.Width / 2, -image.Size.Height / 2, image.Size.Height, image.Size.Width), image.CGImage);
                }
                else
                {
                    //bitmap?.draw(self.cgImage!, in: CGRect(x: -size.width / 2, y: -size.height / 2, width: size.width, height: size.height))                        
                    context.DrawImage(new CGRect(-image.Size.Width / 2, -image.Size.Height / 2, image.Size.Width, image.Size.Height), image.CGImage);
                }
            }
            var fixedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return fixedImage;
        }

        private UIImage FixImageRotation(UIImage image)
        {
            float radians = 0;// -(float)(System.Math.PI / 2.0f);

            var size = new CGRect(new CGPoint(0, 0), image.Size);
            var transform = CGAffineTransform.MakeRotation(radians);
            var rotatedRect = CGAffineTransform.CGRectApplyAffineTransform(size, transform);
            var rotatedSize = rotatedRect.Integral().Size;

            UIGraphics.BeginImageContext(rotatedSize);
            using (var context = UIGraphics.GetCurrentContext())
            {
                var origin = new CGPoint(rotatedSize.Width / 2.0, rotatedSize.Height / 2.0);
                context.TranslateCTM(origin.X, origin.Y);
                context.RotateCTM(radians);
                image.Draw(new CGRect(x: -origin.X, y: -origin.Y, width: size.Width, height: size.Height));

                var rotatedImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
                return rotatedImage;
            }
        }

        private void fillPixelBufferFromImage(UIImage image, CVPixelBuffer pixelBuffer)
        {
            pixelBuffer.Lock(CVPixelBufferLock.None);

            var pixelData = pixelBuffer.BaseAddress;
            var rgbColorSpace = CGColorSpace.CreateDeviceRGB();

            var width = pixelBuffer.Width;
            var height = pixelBuffer.Height;

            var bitsPerComponent = 8;
            var bytesPerPixel = 4;

            nint bytesPerRow = width * bytesPerPixel;

            try
            {

                using (var context = new CGBitmapContext(pixelData, width, height, bitsPerComponent, bytesPerRow, rgbColorSpace, CGBitmapFlags.PremultipliedFirst))
                {
                    context.DrawImage(new CGRect(0, 0, width, height), image.CGImage);
                }
            }
            catch (Exception ex)
            {

            }


            pixelBuffer.Unlock(CVPixelBufferLock.None);

        }

    }
}