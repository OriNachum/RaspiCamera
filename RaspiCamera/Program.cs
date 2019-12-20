using Unosquare.RaspberryIO.Camera;
using System;
using System.IO;
using System.Threading;

namespace RaspiCamera
{
    class Program
    {
        private const string FilePath = "test.jpeg";
        private const string VideoFilePathFormat = "test{0}.jpeg";

        static void Main(string[] args)
        {
            var imageJpeg = CameraController.Instance.CaptureImageJpeg(1024, 768);
            using (var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(imageJpeg, 0, imageJpeg.Length);
            }
            Console.WriteLine($"Wrote image to: {FilePath}");

            var millisecondsSinceYearStart = (new DateTime(DateTime.UtcNow.Year, 0, 0) - DateTime.UtcNow).TotalMilliseconds;
            string videoFilePath = string.Format(VideoFilePathFormat, millisecondsSinceYearStart);
            var bytesWrittenSoFar = 0;
            CameraController.Instance.OpenVideoStream(x =>
            {
                using (var fs = new FileStream(videoFilePath, FileMode.Create, FileAccess.Write))
                {
                    var chunkSize = x.Length;
                    fs.Write(x, bytesWrittenSoFar, chunkSize);
                    bytesWrittenSoFar += chunkSize;
                }
            });
            Thread.Sleep(3000);
            CameraController.Instance.CloseVideoStream();
            Console.WriteLine($"Wrote video to: {videoFilePath}");

        }
    }
}
