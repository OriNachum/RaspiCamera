using MMALSharp;
using MMALSharp.Handlers;
using MMALSharp.Native;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaspiCamera.Impl
{
    public class MMALSharpCamera : IRaspiCamera
    {
        public async Task TakePictureAsync(string filePath)
        {
            // Singleton initialized lazily. Reference once in your application.
            MMALCamera cam = MMALCamera.Instance;

            using (var imgCaptureHandler = new ImageStreamCaptureHandler(filePath))
            {
                await cam.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
            }

            // Cleanup disposes all unmanaged resources and unloads Broadcom library. To be called when no more processing is to be done
            // on the camera.
            cam.Cleanup();
        }

        public async Task TakeVideoAsync(string filePath)
        {
            // Singleton initialized lazily. Reference once in your application.
            MMALCamera cam = MMALCamera.Instance;

            // using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/", "avi"))
            using (var vidCaptureHandler = new VideoStreamCaptureHandler(filePath))
            {
                var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3));

                await cam.TakeVideo(vidCaptureHandler, cts.Token);
            }

            // Cleanup disposes all unmanaged resources and unloads Broadcom library. To be called when no more processing is to be done
            // on the camera.
            cam.Cleanup();
        }
    }
}
