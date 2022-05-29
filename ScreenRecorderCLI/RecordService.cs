using ScreenRecorderLib;
using System;
using System.Collections.Generic;
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
        public void CreateRecording(Options opt)
        {


            var recordOpt = new RecorderOptions
            {
                OutputOptions = new OutputOptions
                {
                    RecorderMode = RecorderMode.Video,
                },
                AudioOptions = new AudioOptions
                {
                    Bitrate = AudioBitrate.bitrate_128kbps,
                    Channels = AudioChannels.Stereo,
                    IsAudioEnabled = !opt.NoAudio,
                },
                VideoEncoderOptions = new VideoEncoderOptions
                {
                    Bitrate = opt.Bitrate * 1000,
                    Framerate = opt.Framerate,
                    IsFixedFramerate = false,
                    Encoder = new H264VideoEncoder
                    {
                        BitrateMode = H264BitrateControlMode.UnconstrainedVBR,
                        EncoderProfile = H264Profile.Main,
                    },
                    //Fragmented Mp4 allows playback to start at arbitrary positions inside a video stream,
                    //instead of requiring to read the headers at the start of the stream.
                    IsFragmentedMp4Enabled = true,
                    //If throttling is disabled, out of memory exceptions may eventually crash the program,
                    //depending on encoder settings and system specifications.
                    IsThrottlingDisabled = false,
                    //Hardware encoding is enabled by default.
                    IsHardwareEncodingEnabled = true,
                    //Low latency mode provides faster encoding, but can reduce quality.
                    IsLowLatencyEnabled = true,
                    //Fast start writes the mp4 header at the beginning of the file, to facilitate streaming.
                    IsMp4FastStartEnabled = false,
                },
                MouseOptions = new MouseOptions
                {
                    IsMouseClicksDetected = false,
                    IsMousePointerEnabled = true
                }
            };

            var sources = new List<RecordingSourceBase>();
            if (opt.WindowTitle != "")
            {
                var windows = Recorder.GetWindows();
                var target = windows.Find(w => w.Title.StartsWith(opt.WindowTitle));
                if (target != null)
                {
                    sources.Add(target);
                    Console.WriteLine($"Recording Target: {target}");
                }
                else
                {
                    Console.Error.WriteLine($"Failed to find the window: {opt.WindowTitle}");
                    return;
                }
            }
            else
            {
                var mainMonitor = new DisplayRecordingSource(DisplayRecordingSource.MainMonitor);
                sources.Add(mainMonitor);
                Console.WriteLine($"Recording Target: {mainMonitor}");
            }
            recordOpt.SourceOptions = new SourceOptions
            {
                RecordingSources = sources
            };

            Thread.Sleep(5000);
            Console.WriteLine("Start recording");

            _rec = Recorder.CreateRecorder(recordOpt);
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
            while (isEncording)
            {
                Thread.Sleep(1000);
            }
        }
        private void Rec_OnRecordingComplete(object sender, RecordingCompleteEventArgs e)
        {
            var path = e.FilePath;
            Console.WriteLine(path);
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
