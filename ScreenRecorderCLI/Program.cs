using System;
using System.IO;
using System.Threading;

namespace ScreenRecorderCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, eventArgs) => {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };

            var fileName = Path.ChangeExtension(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), "mp4");
            var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fileName);

            var recordService = new RecordService(path);

            recordService.CreateRecording();

            exitEvent.WaitOne();
            recordService.EndRecording();
        }
    }
}
