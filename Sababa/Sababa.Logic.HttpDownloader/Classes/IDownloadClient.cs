using System;
using System.Threading.Tasks;

namespace Sababa.Logic.HttpDownloader.Classes
{
    public interface IDownloadClient
    {
        bool AddFile(string fileName, string uri);
        bool DeleteFile(string fileName);
        Task<bool> StartDownload(string fileName, IProgress<DownloadArgs> progress = null);
        void StopDownload(string fileName);
    }
}
