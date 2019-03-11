using System;
using System.IO;
using System.Threading;
using CommandLine;

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

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opt => {
                    var recordService = new RecordService(opt.FilePath);
                    recordService.CreateRecording(opt);

                    exitEvent.WaitOne();
                    recordService.EndRecording();
                })
                .WithNotParsed(er => {
                    Console.Error.WriteLine("Invalid arguments. See --help");
                    return;
                });
        }
    }
}
