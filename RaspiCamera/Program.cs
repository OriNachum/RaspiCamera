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

            //await TestUnobscureCameraAsync(pictureFilePath, videoFilePath);
            await TestMMALSharpCameraAsync(pictureFilePath, videoFilePath);
            //await TestPythonAsync(pictureFilePath, videoFilePath);
        }

        private static async Task TestPythonAsync(string pictureFilePath, string videoFilePath)
        {
            try
            {
                using (var camera = new PythonCamera())
                {
                    await camera.TakePictureAsync($"Python.{pictureFilePath}");
                    await camera.TakeVideoAsync($"Python.{videoFilePath}");

                    Console.WriteLine($"{nameof(Program)} {nameof(Main)} {nameof(PythonCamera)} Completed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(Program)} {nameof(Main)} {nameof(PythonCamera)} Failed");
                Console.WriteLine($"{nameof(Program)} {nameof(Main)} {ex.ToString()}");
                Console.WriteLine($"{nameof(Program)} {nameof(Main)} {nameof(PythonCamera)} Failed");
            }
        }

        private static async Task TestMMALSharpCameraAsync(string pictureFilePath, string videoFilePath)
        {
            try
            {
                using (var camera = new MMALSharpCamera())
                {
                    await camera.TakePictureAsync($"MMALSharp.{pictureFilePath}");
                    await camera.TakeVideoAsync($"MMALSharp.{videoFilePath}");
                    await camera.TakePicturesAsync($"MMALSharp.PictureStream.{PictureFilePathFormat}", TimeSpan.FromSeconds(30), 250);
                    Console.WriteLine($"{nameof(Program)} {nameof(Main)} {nameof(MMALSharpCamera)} Completed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(Program)} {nameof(Main)} {nameof(MMALSharpCamera)} Failed");
                Console.WriteLine($"{nameof(Program)} {nameof(Main)} {ex.ToString()}");
                Console.WriteLine($"{nameof(Program)} {nameof(Main)} {nameof(MMALSharpCamera)} Failed");
            }
        }

        private static async Task TestUnobscureCameraAsync(string pictureFilePath, string videoFilePath)
        {
            try
            {
                using (var camera = new UnobscureCamera())
                {
                    await camera.TakePictureAsync($"Unobscure.{pictureFilePath}");
                    await camera.TakeVideoAsync($"Unobscure.{videoFilePath}");

                    Console.WriteLine($"{nameof(Program)} {nameof(Main)} {nameof(UnobscureCamera)} Completed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(Program)} {nameof(Main)} {nameof(UnobscureCamera)} Failed");
                Console.WriteLine($"{nameof(Program)} {nameof(Main)} {ex.ToString()}");
                Console.WriteLine($"{nameof(Program)} {nameof(Main)} {nameof(UnobscureCamera)} Failed");
            }
        }
    }
}
