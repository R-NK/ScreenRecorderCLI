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
                return Utils.GenerateSavePath(_filePath);
            }
            set
            {
                _filePath = value;
            }
        }
        private string _filePath;
    }
}
