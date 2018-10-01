using System.Threading;

namespace Sababa.Logic.HttpDownloader.Classes
{
    public interface IDownloadFiles
    {
        bool AddFile(string fileName, string uri);
        bool DeleteFile(string fileName);
        bool UpdateStatus(string fileName, StatusOfDownload state);
        bool Contains(string fileName);
        string GetUri(string fileName);
        CancellationTokenSource GetCancellationTokenSource(string fileName);
        StatusOfDownload GetStatus(string fileName);
        int Count();
        void SaveCurrentState(string path);
        void ReadFromFile(string path);
    }
}
