using MMALSharp.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspiCamera.Impl
{
    public class RunningImageStreamCaptureHandler : ImageStreamCaptureHandler
    {
        long index = 0;

        public RunningImageStreamCaptureHandler(string filePath) : base(filePath)
        {

        }

        public RunningImageStreamCaptureHandler(string directoryPath, string fileExtension) : base(directoryPath, fileExtension)
        {

        }

        public override void NewFile()
        {
            string baseFileName = this.CurrentFilename;
            this.CurrentFilename = $"{baseFileName.Substring(0, baseFileName.Length-4)}.{index++}.{baseFileName.Substring(baseFileName.Length - 3, 3)}";
            base.NewFile();
            this.CurrentFilename = baseFileName;
        }
    }
}
