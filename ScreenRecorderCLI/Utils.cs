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
        public static string GenerateSavePath(string fileNameWithOutExt = null)
        {
            var fileName = Path.ChangeExtension(fileNameWithOutExt, "mp4") ?? GenerateFileNameByDateTimeNow();
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recorded", fileName);
        }
        public static string GenerateFileNameByDateTimeNow()
        {
            return Path.ChangeExtension(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), "mp4");
        }
    }
}
