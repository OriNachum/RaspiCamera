using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaspiCamera
{
    interface IRaspiCamera
    {
        Task TakeVideoAsync(string filePath);

        Task TakePictureAsync(string filePath);
    }
}
