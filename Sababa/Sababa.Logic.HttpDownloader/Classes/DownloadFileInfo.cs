using System.Threading;
using Newtonsoft.Json;

namespace Sababa.Logic.HttpDownloader.Classes
{
    public class DownloadFileInfo
    {
        public string URI { get; set; }
        public StatusOfDownload StatusOfDownload { get; set; }
        [JsonIgnore]
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
