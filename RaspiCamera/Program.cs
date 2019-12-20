using Unosquare.RaspberryIO.Camera;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RaspiCamera
{
    class Program
    {
        private const string FilePath = "test.jpeg";
        private const string VideoFilePathFormat = "test.{0}.jpeg";

        static async Task Main(string[] args)
        {
            await TakePictureAsync();

            await TakeVideoAsync();

        }

        private static async Task TakeVideoAsync()
        {
            var millisecondsSinceYearStart = (DateTime.UtcNow - new DateTime(DateTime.UtcNow.Year, 1, 1)).TotalMilliseconds;
            string videoFilePath = string.Format(VideoFilePathFormat, millisecondsSinceYearStart);
            var bytesWrittenSoFar = 0;
            using (var fs = new FileStream(videoFilePath, FileMode.Create, FileAccess.Write))
            {
                CameraController.Instance.OpenVideoStream(x =>
                {
                    var chunkSize = x.Length;
                    fs.Write(x, bytesWrittenSoFar, chunkSize);
                    bytesWrittenSoFar += chunkSize;
                }, () =>
                {
                    fs.Flush(flushToDisk: true);
                });
            }
            await Task.FromResult(1).ContinueWith((x) => 
            {
                Thread.Sleep(3000);
                return 1;
            });
            CameraController.Instance.CloseVideoStream();
            Console.WriteLine($"Wrote video to: {videoFilePath}");
        }

        private static async Task TakePictureAsync()
        {
            var imageJpeg = await CameraController.Instance.CaptureImageJpegAsync(1024, 768);
            using (var fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(imageJpeg, 0, imageJpeg.Length);
                fs.Flush(flushToDisk: true);
            }
            Console.WriteLine($"Wrote image to: {FilePath}, size of {imageJpeg.Length}");
        }
    }
}
