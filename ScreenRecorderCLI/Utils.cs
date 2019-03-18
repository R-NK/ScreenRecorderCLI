using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ScreenRecorderCLI
{
    class Utils
    {
        public static string GenerateSavePath()
        {
            var fileName = Path.ChangeExtension(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), "mp4");
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recorded", fileName);
        }
    }
}
