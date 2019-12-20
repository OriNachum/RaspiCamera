using MMALSharp.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspiCamera.Impl
{
    public class IndexedImageStreamCaptureHandler : ImageStreamCaptureHandler
    {
        int index = 0;
        public IndexedImageStreamCaptureHandler(string filename) : base(filename)
        {

        }

        public IndexedImageStreamCaptureHandler(string directory, string extension) : base(directory, extension)
        {

        }

        public override void NewFile()
        {
            Console.WriteLine($"Creating stream for: Directory: {this.Directory}; Filename: {this.CurrentFilename}; Extension: {this.Extension}");
            base.NewFile();
            Console.WriteLine($"Writing to: {this.CurrentStream.Name}"); 
        }
    }
}
