using Unosquare.RaspberryIO.Camera;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp;
using RaspiCamera.Impl;

namespace RaspiCamera
{
    class Program
    {
        private const string PictureFilePathFormat = "picture.{0}.jpeg";
        private const string VideoFilePathFormat = "video.{0}.avi";

        static async Task Main(string[] args)
        {
            var millisecondsSinceYearStart = (DateTime.UtcNow - new DateTime(DateTime.UtcNow.Year, 1, 1)).TotalMilliseconds;
            string pictureFilePath = string.Format(PictureFilePathFormat, millisecondsSinceYearStart);
            string videoFilePath = string.Format(VideoFilePathFormat, millisecondsSinceYearStart);

            IRaspiCamera camera = new UnobscureCamera();
            await camera.TakePictureAsync($"Unobscure.{pictureFilePath}");
            await camera.TakeVideoAsync($"Unobscure.{videoFilePath}");

            camera = new MMALSharpCamera();
            await camera.TakePictureAsync($"MMALSharp.{pictureFilePath}");
            await camera.TakeVideoAsync($"MMALSharp.{videoFilePath}");
        }
    }
}
