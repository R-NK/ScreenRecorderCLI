using System;
using System.Collections.Generic;
using System.Text;
using ScreenRecorderLib;
using System.Threading;

namespace ScreenRecorderCLI
{
    class RecordService
    {
        private Recorder _rec;
        private string _path;
        private bool isEncording;

        public RecordService(string path)
        {
            _path = path;
        }
        public void CreateRecording()
        {
            _rec = Recorder.CreateRecorder();
            _rec.OnRecordingComplete += Rec_OnRecordingComplete;
            _rec.OnRecordingFailed += Rec_OnRecordingFailed;
            _rec.OnStatusChanged += Rec_OnStatusChanged;
            //Record to a file
            _rec.Record(_path);
        }
        public void EndRecording()
        {
            isEncording = true;
            _rec.Stop();
            while (isEncording) {
                Thread.Sleep(1000);
            }
        }
        private void Rec_OnRecordingComplete(object sender, RecordingCompleteEventArgs e)
        {
            var path = e.FilePath;
            Console.WriteLine($"file is saved to: {path}");
            isEncording = false;
        }
        private void Rec_OnRecordingFailed(object sender, RecordingFailedEventArgs e)
        {
            var error = e.Error;
            Console.Error.WriteLine(error);
        }
        private void Rec_OnStatusChanged(object sender, RecordingStatusEventArgs e)
        {
            switch (e.Status)
            {
                case RecorderStatus.Idle:
                    //Console.WriteLine("Recorder is idle");
                    break;
                case RecorderStatus.Recording:
                    Console.WriteLine("Recording started");
                    Console.WriteLine("Press Ctrl-C to stop recording");
                    break;
                case RecorderStatus.Paused:
                    Console.WriteLine("Recording paused");
                    break;
                case RecorderStatus.Finishing:
                    Console.WriteLine("Finishing encoding");
                    break;
                default:
                    break;
            }
        }
    }
}
