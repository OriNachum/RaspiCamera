using MMALSharp;
using MMALSharp.Common.Utility;
using MMALSharp.Config;
using MMALSharp.Handlers;
using MMALSharp.Native;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaspiCamera.Impl
{
    public class MMALSharpCamera : IRaspiCamera, IDisposable
    {
        private const string ImageExt = "jpg";

        public MMALCamera MMALSharpCameraInstance { get; }

        public MMALSharpCamera()
        {
            this.MMALSharpCameraInstance = MMALCamera.Instance;
        }

        public async Task TakePicturesAsync(string directory, TimeSpan duration, int msWaitBetweenPictures)
        {
            MMALCameraConfig.StillResolution = new Resolution(1080, 920);
            try
            {
                // Singleton initialized lazily. Reference once in your application.
                MMALCamera cam = this.MMALSharpCameraInstance;
                using (var imgCaptureHandler = new ImageStreamCaptureHandler(directory, ImageExt))
                {
                    var cts = new CancellationTokenSource(duration);
                    var timelapse = new Timelapse
                    {
                        Mode = TimelapseMode.Millisecond,
                        Value = msWaitBetweenPictures,
                        CancellationToken = cts.Token
                    };
                    await cam.TakePictureTimelapse(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420, timelapse);
                }

                // Cleanup disposes all unmanaged resources and unloads Broadcom library. To be called when no more processing is to be done
                // on the camera.
                Console.WriteLine($"Wrote picture to: {directory} with ext. {ImageExt}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(MMALSharpCamera)} {nameof(TakePictureAsync)} Failed");
                Console.WriteLine($"{nameof(MMALSharpCamera)} {nameof(TakePictureAsync)} {ex.ToString()}");
                Console.WriteLine($"{nameof(MMALSharpCamera)} {nameof(TakePictureAsync)} Failed");
            }
        }


        public async Task TakePictureAsync(string filePath)
        {
            try
            {
                // Singleton initialized lazily. Reference once in your application.
                MMALCamera cam = this.MMALSharpCameraInstance;

                using (var imgCaptureHandler = new ImageStreamCaptureHandler(filePath))
                {
                    await cam.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
                }

                // Cleanup disposes all unmanaged resources and unloads Broadcom library. To be called when no more processing is to be done
                // on the camera.
                Console.WriteLine($"Wrote picture to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(MMALSharpCamera)} {nameof(TakePictureAsync)} Failed");
                Console.WriteLine($"{nameof(MMALSharpCamera)} {nameof(TakePictureAsync)} {ex.ToString()}");
                Console.WriteLine($"{nameof(MMALSharpCamera)} {nameof(TakePictureAsync)} Failed");
            }
        }

        public async Task TakeVideoAsync(string filePath)
        {
            try
            {
                // Singleton initialized lazily. Reference once in your application.
                MMALCamera cam = this.MMALSharpCameraInstance;

                // using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/", "avi"))
                using (var vidCaptureHandler = new VideoStreamCaptureHandler(filePath))
                {
                    
                    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                    var split = new Split
                    {
                        Mode = TimelapseMode.Second,
                        Value = 15,
                    };
                    await cam.TakeVideo(vidCaptureHandler, cts.Token, split);
                }

                // Cleanup disposes all unmanaged resources and unloads Broadcom library. To be called when no more processing is to be done
                // on the camera.
                cam.Cleanup();
                Console.WriteLine($"Wrote video to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(MMALSharpCamera)} {nameof(TakeVideoAsync)} Failed");
                Console.WriteLine($"{nameof(MMALSharpCamera)} {nameof(TakeVideoAsync)} {ex.ToString()}");
                Console.WriteLine($"{nameof(MMALSharpCamera)} {nameof(TakeVideoAsync)} Failed");
            }
        }

        public void Dispose()
        {
            this.MMALSharpCameraInstance.Cleanup();
        }
    }
}
