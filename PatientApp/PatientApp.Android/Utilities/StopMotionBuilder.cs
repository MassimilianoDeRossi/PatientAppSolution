using System;
using System.Collections.Generic;
using Android.Media;
using Android.Graphics;
using Java.IO;
using Java.Nio;
using Android.OS;
using Android.Graphics.Drawables;

namespace PatientApp.Droid.Utilities
{
    /// <summary>
    /// Class that allow user to build a timelapse video using MediaCodec and MediaMuxer.
    /// ATTENTION: it works only with yuv420semiplanar conversion
    /// </summary>
    public class StopMotionBuilder
    {
        private MediaCodec _mediaCodec;
        private MediaMuxer _mediaMuxer;
        private AndroidTimeLapseParameters parameters;

        private Handler aHandler = new Handler();

        private int _trackIndex;
        private int _totalFrames = 0;
        private byte[] _frameCached;

        private Action<long, long> _progressCallback;

        /// <summary>
        /// Build a timelapse video with given parameters
        /// </summary>
        public void Build(AndroidTimeLapseParameters param, Action<long, long> progressCallback, Action<string> successCallback, Action<string> failureCallback)
        {
            if (string.IsNullOrEmpty(param.VideoOutPath))
            {
                failureCallback("Video path is not set.");
                return;
            }
            if (param.PhotoUrls == null || param.PhotoUrls.Count == 0)
            {
                failureCallback("Photo urls are not set.");
                return;
            }

            parameters = param;
            parameters.UpdateSize();

            _progressCallback = progressCallback;

            _totalFrames = parameters.FrameRate * parameters.PhotoUrls.Count;

            if (CreateVideo())
            {
                successCallback?.Invoke(parameters.VideoOutPath);
            }
            else
            {
                failureCallback?.Invoke("Cannot create video.");
            }
        }

        /// <summary>
        /// Try to create video and return status
        /// </summary>
        public bool CreateVideo()
        {
            try
            {
                if (!PrepareEncoder())
                {
                    return false;
                }
                Encode();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error during encoding timelapse. Error: " + ex.Message);
                return false;
            }
            finally
            {
                Release();
            }
        }

        /// <summary>
        /// Initialize all resources to encoder video and create frames
        /// </summary>
        private bool PrepareEncoder()
        {
            try
            {
                MediaCodecInfo codecInfo = SelectCodec(parameters.MimeType);
                int colorFormat = SelectColorFormat(codecInfo, parameters.MimeType);

                MediaFormat mediaFormat = MediaFormat.CreateVideoFormat(parameters.MimeType, parameters.VideoWidth, parameters.VideoHeight);
                mediaFormat.SetInteger(MediaFormat.KeyBitRate, parameters.BitRate);
                mediaFormat.SetInteger(MediaFormat.KeyFrameRate, parameters.FrameRate);
                mediaFormat.SetInteger(MediaFormat.KeyColorFormat, colorFormat);
                mediaFormat.SetInteger(MediaFormat.KeyIFrameInterval, parameters.IFrameInterval);

                _mediaCodec = MediaCodec.CreateByCodecName(codecInfo.Name);
                _mediaCodec.Configure(mediaFormat, null, null, MediaCodecConfigFlags.Encode);
                _mediaCodec.Start();

                _mediaMuxer = new MediaMuxer(parameters.VideoOutPath, MuxerOutputType.Mpeg4);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a color format that is supported by the codec and by this test
        /// code.If no match is found, this throws a test failure -- the set of
        /// formats known to the test should be expanded for new platforms.
        /// </summary>
        private int SelectColorFormat(MediaCodecInfo codecInfo, string mimeType)
        {
            try
            {
                MediaCodecInfo.CodecCapabilities capabilities = codecInfo
                        .GetCapabilitiesForType(mimeType);
                for (int i = 0; i < capabilities.ColorFormats.Count; i++)
                {
                    int colorFormat = capabilities.ColorFormats[i];
                    if (IsRecognizedFormatHighPriority(colorFormat))
                    {
                        return colorFormat;
                    }
                }
                //low priority cicle
                for (int i = 0; i < capabilities.ColorFormats.Count; i++)
                {
                    int colorFormat = capabilities.ColorFormats[i];
                    if (IsRecognizedFormatLowPriority(colorFormat))
                    {
                        return colorFormat;
                    }
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Cannot get the color, set default Formatyuv420semiplanar");
            }
            return (int)MediaCodecCapabilities.Formatyuv420semiplanar;
        }

        /// <summary>
        /// Returns the first codec capable of encoding the specified MIME type, or null if no match was found.
        /// </summary>
        private MediaCodecInfo SelectCodec(string mimeType)
        {
            int numCodecs = MediaCodecList.CodecCount;
            for (int i = 0; i < numCodecs; i++)
            {
                MediaCodecInfo codecInfo = MediaCodecList.GetCodecInfoAt(i);
                if (!codecInfo.IsEncoder)
                {
                    continue;
                }
                string[] types = codecInfo.GetSupportedTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (types[j].Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                    {
                        return codecInfo;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns true if this is a color format that this test code understands
        /// (i.e. we know how to read and generate frames in this format).
        /// </summary>
        private bool IsRecognizedFormatHighPriority(int colorFormat)
        {
            switch (colorFormat)
            {
                // these are the formats we know how to handle for
                //case (int)MediaCodecCapabilities.Formatyuv420planar:
                //case (int)MediaCodecCapabilities.Formatyuv420packedplanar:
                case (int)MediaCodecCapabilities.Formatyuv420semiplanar:
                //case (int)MediaCodecCapabilities.Formatyuv420packedsemiplanar:
                //case (int)MediaCodecCapabilities.TiFormatyuv420packedsemiplanar:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true if this is a color format that this test code understands
        /// (i.e. we know how to read and generate frames in this format).
        /// </summary>
        private bool IsRecognizedFormatLowPriority(int colorFormat)
        {
            switch (colorFormat)
            {
                // these are the formats we know how to handle for
                case (int)MediaCodecCapabilities.Formatyuv420planar:
                //case (int)MediaCodecCapabilities.Formatyuv420packedplanar:
                //case (int)MediaCodecCapabilities.Formatyuv420semiplanar:
                //case (int)MediaCodecCapabilities.Formatyuv420packedsemiplanar:
                //case (int)MediaCodecCapabilities.TiFormatyuv420packedsemiplanar:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Create frames and encode the video
        /// </summary>
        private void Encode()
        {
            int inputFrames = 0;
            int encodedFrames = 0;
            while (encodedFrames <= _totalFrames + 1)
            {
                if (inputFrames <= _totalFrames + 1) // we put 2 fake frame at the end to avoid problem with video lenght
                {
                    long ptsUsec = ComputePresentationTime(inputFrames);

                    int inputBufIndex = _mediaCodec.DequeueInputBuffer(parameters.TimeoutWaitBuffer);
                    if (inputBufIndex >= 0)
                    {
                        var frameBuffer = GenerateFrame(inputFrames, _totalFrames);
                        var inputBuffer = _mediaCodec.GetInputBuffer(inputBufIndex);
                        inputBuffer.Clear();
                        inputBuffer.Put(frameBuffer);
                        _mediaCodec.QueueInputBuffer(inputBufIndex, 0, frameBuffer.Length, ptsUsec, MediaCodecBufferFlags.SyncFrame); // MediaCodecBufferFlags.SyncFrame could be replace with 0 if it's too slow
                        inputFrames++;

                        _progressCallback?.Invoke(inputFrames, _totalFrames);
                        System.Console.WriteLine("MEDIAENCODER: " + inputFrames + "/" +  _totalFrames);
                    }
                }

                MediaCodec.BufferInfo mBufferInfo = new MediaCodec.BufferInfo();
                int encoderStatus = _mediaCodec.DequeueOutputBuffer(mBufferInfo, parameters.TimeoutWaitBuffer);

                if (encoderStatus == (int) MediaCodecInfoState.TryAgainLater)
                {
                    System.Console.WriteLine("No output from encoder available"); //wait
                }
                else if (encoderStatus == (int)MediaCodecInfoState.OutputBuffersChanged)
                {
                    // not expected for an encoder
                    //TODO What implement in this case?
                }
                else if (encoderStatus == (int)MediaCodecInfoState.OutputFormatChanged)
                {
                    // not expected for an encoder
                    MediaFormat newFormat = _mediaCodec.OutputFormat;
                    _trackIndex = _mediaMuxer.AddTrack(newFormat);
                    _mediaMuxer.Start();
                }
                else if (encoderStatus < 0)
                {
                    throw new Exception("unexpected result from encoder.dequeueOutputBuffer: " + encoderStatus);
                }
                else // encoderStatus >= 0
                {
                    if (mBufferInfo.Size != 0)
                    {
                        ByteBuffer encodedData = _mediaCodec.GetOutputBuffer(encoderStatus);
                        if (encodedData == null)
                        {
                            throw new Exception("encoderOutputBuffer " + encoderStatus + " was null");
                        }

                        mBufferInfo.PresentationTimeUs = ComputePresentationTime(encodedFrames);
                        encodedData.Position(mBufferInfo.Offset);
                        encodedData.Limit(mBufferInfo.Offset + mBufferInfo.Size);

                        _mediaMuxer.WriteSampleData(_trackIndex, encodedData, mBufferInfo);
                        _mediaCodec.ReleaseOutputBuffer(encoderStatus, false);

                        encodedFrames++;
                        System.Console.WriteLine("MEDIAENCODER encoded: " + encodedFrames + "/" + _totalFrames);
                        System.Console.WriteLine("MEDIAENCODER encoded PS: " + mBufferInfo.PresentationTimeUs);
                    }
                }
            }
        }

        /// <summary>
        /// Generate a frame encoding a bitmap and converting colors from ARGB to YUV420 (HDR).
        /// Because of every image will during 1 second, we need to create a number of frames equals to 'frameRate'.
        /// To improve performance we calculate only one time the frame, and the other 'frameRate' - 1 are cached.
        /// </summary>
        private byte[] GenerateFrame(int frameIndex, int totalFrames)
        {
            if (frameIndex < totalFrames && frameIndex % parameters.FrameRate == 0)
            {
                var imageBytes = System.IO.File.ReadAllBytes(parameters.PhotoUrls[frameIndex / parameters.FrameRate]);
                var tempBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                _frameCached = GetNV21(tempBitmap.Width, tempBitmap.Height, tempBitmap);
                return _frameCached;
            }
            else
            {
                return _frameCached;
            }
        }

        /// <summary>
        /// Decode and image and convert it to YUV420SemiPlanar (commonly known as NV21) and dispose the bitmap passed as parameter
        /// </summary>
        private byte[] GetNV21(int inputWidth, int inputHeight, Bitmap scaled)
        {
            int[] argb = new int[inputWidth * inputHeight];
            scaled.GetPixels(argb, 0, inputWidth, 0, 0, inputWidth, inputHeight);

            byte[] yuv = new byte[inputWidth * inputHeight * 3 / 2];
            EncodeYUV420SP(yuv, argb, inputWidth, inputHeight);

            scaled.Recycle();

            return yuv;
        }

        /// <summary>
        /// Convert byte array from argb to yuv420semiplanar
        /// </summary>
        private void EncodeYUV420SP(byte[] yuv420sp, int[] argb, int width, int height)
        {
            int frameSize = width * height;

            int yIndex = 0;
            int uvIndex = frameSize;

            long R, G, B, Y, U, V;
            int index = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    R = (argb[index] & 0xff0000) >> 16;
                    G = (argb[index] & 0xff00) >> 8;
                    B = (argb[index] & 0xff) >> 0;


                    Y = ((66 * R + 129 * G + 25 * B + 128) >> 8) + 16;
                    U = ((-38 * R - 74 * G + 112 * B + 128) >> 8) + 128;
                    V = ((112 * R - 94 * G - 18 * B + 128) >> 8) + 128;


                    yuv420sp[yIndex++] = (byte)((Y < 0) ? 0 : ((Y > 255) ? 255 : Y));
                    if (j % 2 == 0 && index % 2 == 0)
                    {
                        yuv420sp[uvIndex++] = (byte)((U < 0) ? 0 : ((U > 255) ? 255 : U));
                        yuv420sp[uvIndex++] = (byte)((V < 0) ? 0 : ((V > 255) ? 255 : V));
                    }

                    index++;
                }
            }
        }

        /// <summary>
        /// Calculate presentation time.
        /// What's presentation time? https://en.wikipedia.org/wiki/Presentation_timestamp
        /// </summary>
        private long ComputePresentationTime(long frameIndex)
        {
            return frameIndex * 1000000 / parameters.FrameRate;
        }

        /// <summary>
        /// Dispose mediacodec and mediamuxer object
        /// </summary>
        private void Release()
        {
            if (_mediaCodec != null)
            {
                try
                {
                    _mediaCodec.Stop();
                }
                finally
                {
                    _mediaCodec.Release();
                    _mediaCodec = null;
                }
            }
            if (_mediaMuxer != null)
            {
                try
                {
                    _mediaMuxer.Stop();
                }
                finally
                {
                    _mediaMuxer.Release();
                    _mediaMuxer = null;
                }
            }
        }
    }

    /// <summary>
    /// Store timelapse data
    /// </summary>
    public class AndroidTimeLapseParameters
    {
        /// <summary>
        /// Video width pixels (all images must be of the same size)
        /// </summary>
        public int VideoWidth { get; set; }

        /// <summary>
        /// Video height pixels (all images must be of the same size)
        /// </summary>
        public int VideoHeight { get; set; }

        /// <summary>
        /// List of input photos path
        /// </summary>
        public List<string> PhotoUrls { get; set; }

        /// <summary>
        /// Video output path
        /// </summary>
        public string VideoOutPath { get; set; }

        /// <summary>
        /// Bit rate (is the number of bits that are conveyed or processed every seconds)
        /// </summary>
        public int BitRate { get; set; }

        /// <summary>
        /// Codec type. Use 'PatientApp.Droid.Utilities.VideoTypes' class to choose one.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Seconds between I-Frames
        /// What's an I-Frame? https://it.wikipedia.org/wiki/Tipi_di_fotogramma_nella_compressione_video
        /// </summary>
        public int IFrameInterval { get; set; }

        /// <summary>
        /// Frame per seconds
        /// </summary>
        public int FrameRate { get; set; }

        /// <summary>
        /// Milliseconds waited to get data from buffer
        /// </summary>
        public long TimeoutWaitBuffer { get; set; }

        public AndroidTimeLapseParameters()
        {
            //Default data
            VideoWidth = 1080;
            VideoHeight = 1920;
            PhotoUrls = null;
            MimeType = VideoTypes.H264_AVC;
            IFrameInterval = 10; //same as framerate is better for this video
            FrameRate = 10; //set 1 fps cause bug
            TimeoutWaitBuffer = 10000;
            VideoOutPath = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads),
                           "timelapse" + DateTime.Now.Ticks + ".mp4").ToString(); //used only to debug
            UpdateBitRate();
        }

        /// <summary>
        /// Update bitrate based on resolution
        /// </summary>
        private void UpdateBitRate()
        {
            //formula pixel count x motion factor x 0.07 // motion factor -> 1 = Low Motion -> 4 = High Motion
            int motionFactor = 1;
            BitRate = VideoWidth * VideoHeight * FrameRate * motionFactor / 100 * 7; 
        }

        /// <summary>
        /// Update VideoWidth and VideoHeight based on the first image in PhotoUrls list
        /// </summary>
        public void UpdateSize()
        {
            if (PhotoUrls?.Count > 0)
            {
                Bitmap b = BitmapFactory.DecodeFile(PhotoUrls[0]);
                VideoHeight = b.Height;
                VideoWidth = b.Width;
                UpdateBitRate();
            }
        }
    }

    /// <summary>
    /// List of all video formats supported by Mediacodec
    /// </summary>
    public static class VideoTypes
    {
        /// <summary>
        /// VP8 video(i.e.video in .webm)
        /// </summary>
        public const string VP8 = "video/x-vnd.on2.vp8";

        /// <summary>
        /// VP9 video(i.e.video in .webm)
        /// </summary>
        public const string VP9 = "video/x-vnd.on2.vp9";

        /// <summary>
        /// H.263 video
        /// </summary>
        public const string H263_3GPP = "video/3gpp";

        /// <summary>
        /// H.264/AVC video
        /// </summary>
        public const string H264_AVC = "video/avc";

        /// <summary>
        /// H.265/HEVC video
        /// </summary>
        public const string H265_HEVC = "video/hevc";

        /// <summary>
        /// MPEG4 video
        /// </summary>
        public const string MPEG4 = "video/mp4v-es";
    }
}