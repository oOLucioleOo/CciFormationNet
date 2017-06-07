using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

// Contains common types for AVI format like FourCC
using SharpAvi;
// Contains types used for writing like AviWriter
using SharpAvi.Output;
// Contains types related to encoding like Mpeg4VideoEncoderVcm
using SharpAvi.Codecs;
using System.IO;

namespace VideoCaptureApplication.TestCapture
{
    class VideoRecorder 
    {
        private AviWriter writer { get; set; }
        private string fileName { get; set; }
        private int fps { get; set; }
        private int quality { get; set; }
        private FourCC codec { get; set; }
        private readonly int screenWidth;
        private readonly int screenHeight;
        private readonly Thread screenThread;
        private readonly Thread screenStreamThread;
        private IAviVideoStream videoStream { get; set; }
        private readonly ManualResetEvent stopThread = new ManualResetEvent(false);
        private readonly ManualResetEvent stopThreadChunk = new ManualResetEvent(false);
        private readonly AutoResetEvent videoFrameWritten = new AutoResetEvent(false);
        


        public VideoRecorder(string fileName,FourCC codec, int quality, int fps)
        {
            System.Windows.Media.Matrix toDevice;
            using (var source = new HwndSource(new HwndSourceParameters()))
            {
                toDevice = source.CompositionTarget.TransformToDevice;
            }

            screenWidth = (int)Math.Round(SystemParameters.PrimaryScreenWidth * toDevice.M11);
            screenHeight = (int)Math.Round(SystemParameters.PrimaryScreenHeight * toDevice.M22);

            // Create AVI writer and specify FPS
            writer = new AviWriter(fileName)
            {
                FramesPerSecond = fps,
                EmitIndex1 = true,
            };

            this.fileName = fileName;
            this.codec = codec;
            this.fps = fps;
            this.quality = quality;

            // Create video stream
            videoStream = CreateVideoStream(codec, quality);
            // Set only name. Other properties were when creating stream, 
            // either explicitly by arguments or implicitly by the encoder used
            videoStream.Name = "Screencast";

            screenThread = new Thread(RecordScreen)
            {
                Name = typeof(VideoRecorder).Name + ".RecordScreen",
                IsBackground = true
            };

            screenThread.Start();
        }

        private IAviVideoStream CreateVideoStream(FourCC codec, int quality)
        {
            // Select encoder type based on FOURCC of codec
            if (codec == KnownFourCCs.Codecs.Uncompressed)
            {
                return writer.AddUncompressedVideoStream(screenWidth, screenHeight);
            }
            else if (codec == KnownFourCCs.Codecs.MotionJpeg)
            {
                return writer.AddMotionJpegVideoStream(screenWidth, screenHeight, quality);
            }
            else
            {
                return writer.AddMpeg4VideoStream(screenWidth, screenHeight, (double)writer.FramesPerSecond,
                    // It seems that all tested MPEG-4 VfW codecs ignore the quality affecting parameters passed through VfW API
                    // They only respect the settings from their own configuration dialogs, and Mpeg4VideoEncoder currently has no support for this
                    quality: quality,
                    codec: codec,
                    // Most of VfW codecs expect single-threaded use, so we wrap this encoder to special wrapper
                    // Thus all calls to the encoder (including its instantiation) will be invoked on a single thread although encoding (and writing) is performed asynchronously
                    forceSingleThreadedAccess: true);
            }
        }

        private void RecordScreen()
        {
            var stopwatch = new Stopwatch();
            var buffer = new byte[screenWidth * screenHeight * 4];
            Task videoWriteTask = null;
            //IAsyncResult videoWriteResult = null;
            var isFirstFrame = true;
            var shotsTaken = 0;
            var timeTillNextFrame = TimeSpan.Zero;
            stopwatch.Start();
            int chunkNumber = 0;

            while (!stopThread.WaitOne(timeTillNextFrame))
            {
                
                while (!stopThreadChunk.WaitOne(timeTillNextFrame))
                {
                    GetScreenshot(buffer);
                    shotsTaken++;

                    // Wait for the previous frame is written
                    if (!isFirstFrame)
                    {

                        videoWriteTask.Wait();

                        //videoStream.EndWriteFrame(videoWriteResult);

                        videoFrameWritten.Set();
                    }

                    // Start asynchronous (encoding and) writing of the new frame
                    videoWriteTask = videoStream.WriteFrameAsync(true, buffer, 0, buffer.Length);

                    //videoWriteResult = videoStream.BeginWriteFrame(true, buffer, 0, buffer.Length, null, null);


                    timeTillNextFrame = TimeSpan.FromSeconds(shotsTaken / (double)writer.FramesPerSecond - stopwatch.Elapsed.TotalSeconds);
                    if (timeTillNextFrame < TimeSpan.Zero)
                        timeTillNextFrame = TimeSpan.Zero;

                    isFirstFrame = false;


                    var file = string.Empty;
                    if (chunkNumber > 0)
                    {
                        file = this.fileName + chunkNumber;
                    }
                    else
                    {
                        file = this.fileName;
                    }


                    var fileInfo = new FileInfo(file);
                    if (fileInfo.Length > 1048576)
                    {
                        stopThreadChunk.Set();
                    }
                }
                stopThreadChunk.Reset();
                chunkNumber++;

                this.writer.Close();
                // Create AVI writer and specify FPS
                this.writer = new AviWriter(fileName+ chunkNumber)
                {
                    FramesPerSecond = this.fps,
                    EmitIndex1 = true,
                };

                this.fileName = fileName;

                // Create video stream
                videoStream = CreateVideoStream(this.codec, this.quality);
                // Set only name. Other properties were when creating stream, 
                // either explicitly by arguments or implicitly by the encoder used
                videoStream.Name = "Screencast";
            }

            stopwatch.Stop();

            // Wait for the last frame is written
            if (!isFirstFrame)
            {

                videoWriteTask.Wait();

                //videoStream.EndWriteFrame(videoWriteResult);

            }
        }

        private void GetScreenshot(byte[] buffer)
        {
            using (var bitmap = new Bitmap(screenWidth, screenHeight))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(screenWidth, screenHeight));
                var bits = bitmap.LockBits(new Rectangle(0, 0, screenWidth, screenHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                bitmap.UnlockBits(bits);

                // Should also capture the mouse cursor here, but skipping for simplicity
                // For those who are interested, look at http://www.codeproject.com/Articles/12850/Capturing-the-Desktop-Screen-with-the-Mouse-Cursor
            }
        }

        public void Dispose()
        {
            stopThread.Set();
            screenThread.Join();

            // Close writer: the remaining data is written to a file and file is closed
            writer.Close();

            stopThread.Close();

            stopThreadChunk.Close();
        }

    }
}
