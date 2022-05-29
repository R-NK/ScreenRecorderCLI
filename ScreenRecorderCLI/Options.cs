using CommandLine;
using System;
using System.IO;

namespace ScreenRecorderCLI
{
    class Options
    {
        [Option('t', "title",
            Required = false,
            Default = "",
            HelpText = "Target window title (Partial Match). When empty, entire desktop will be recorded."),
            ]
        public string WindowTitle { get; set; }

        [Option('f', "framerate",
            Required = true,
            Default = 30,
            HelpText = "Set video framerate")]
        public int Framerate { get; set; }

        [Option('b', "bitrate",
            Required = false,
            Default = 15000,
            HelpText = "Video Bitrate (kbps)")]
        public int Bitrate { get; set; }

        [Option('o', "output",
            Required = false,
            HelpText = "Set output filepath")]
        public string FilePath
        {
            get
            {
                if (_filePath == null) return Utils.GenerateSavePath();

                Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                return Path.GetFullPath(_filePath);
            }
            set
            {
                _filePath = value;
            }
        }
        private string _filePath;

        [Option("noaudio",
            Required = false,
            Default = false,
            HelpText = "Record without audio")]
        public bool NoAudio { get; set; }
    }
}
