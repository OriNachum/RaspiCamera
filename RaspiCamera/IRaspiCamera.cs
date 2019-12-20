using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaspiCamera
{
    interface IRaspiCamera : IDisposable
    {
        Task TakeVideoAsync(string filePath);

        Task TakePictureAsync(string filePath);
    }
}
