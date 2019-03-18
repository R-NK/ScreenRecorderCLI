using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.IO;

namespace ScreenRecorderCLI
{
    class Options
    {
        [Option('f', "framerate",
            Required = true,
            Default = 30,
            HelpText = "Set video framerate")]
        public int Framerate { get; set; }

        [Option('o', "output",
            Required = false,
            HelpText = "Set output filepath")]
        public string FilePath {
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
