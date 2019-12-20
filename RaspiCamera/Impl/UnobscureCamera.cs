using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Camera;

namespace RaspiCamera.Impl
{
    public class UnobscureCamera : IRaspiCamera
    {
        public async Task TakePictureAsync(string filePath)
        {
            try
            {
                var imageJpeg = await CameraController.Instance.CaptureImageJpegAsync(1024, 768);
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    fs.Write(imageJpeg, 0, imageJpeg.Length);
                    fs.Flush(flushToDisk: true);
                }
                Console.WriteLine($"Wrote image to: {filePath}, size of {imageJpeg.Length}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(UnobscureCamera)} {nameof(TakePictureAsync)} Failed");
                Console.WriteLine($"{nameof(UnobscureCamera)} {nameof(TakePictureAsync)} {ex.ToString()}");
                Console.WriteLine($"{nameof(UnobscureCamera)} {nameof(TakePictureAsync)} Failed");
            }
        }
        
        public async Task TakeVideoAsync(string filePath)
        {
            try
            {
                var bytesWrittenSoFar = 0;
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
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
                Console.WriteLine($"Wrote video to: {filePath}, size of {bytesWrittenSoFar}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(UnobscureCamera)} {nameof(TakeVideoAsync)} Failed");
                Console.WriteLine($"{nameof(UnobscureCamera)} {nameof(TakeVideoAsync)} {ex.ToString()}");
                Console.WriteLine($"{nameof(UnobscureCamera)} {nameof(TakeVideoAsync)} Failed");
            }
        }


    }
}
