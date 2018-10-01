using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Sababa.Logic.HttpDownloader.Classes
{
    public class CurrentDownloadFiles : IDownloadFiles
    {
        private ConcurrentDictionary<string, DownloadFileInfo> _files = new ConcurrentDictionary<string, DownloadFileInfo>();

        public bool AddFile(string fileName, string uri)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(uri))
            {
                return false;
            }

            return _files.TryAdd(fileName, new DownloadFileInfo()
            {
                URI = uri,
                StatusOfDownload = StatusOfDownload.Expect,
                CancellationTokenSource = new CancellationTokenSource()
            });
        }

        public bool DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            return _files.TryRemove(fileName, out DownloadFileInfo info);
        }

        public bool Contains(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            return _files.ContainsKey(fileName);
        }

        public bool UpdateStatus(string fileName, StatusOfDownload status)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            _files[fileName].StatusOfDownload = status;
            return true;
        }

        //public string ShowStatusAboutDownloadFiles()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    foreach (var item in _files)
        //    {
        //        sb.Append($"FileName: {item.Key}, URI: {item.Value.URI}, Status: {item.Value.StatusOfDownload};{Environment.NewLine}");
        //    }

        //    return sb.ToString();
        //}

        public string GetUri(string fileName)
        {
            try
            {
                DownloadFileInfo result;
                _files.TryGetValue(fileName, out result);
                return result.URI;
            }
            catch (ArgumentNullException)
            {
                // log
                throw;
            }
        }

        public CancellationTokenSource GetCancellationTokenSource(string fileName)
        {
            try
            {
                DownloadFileInfo result;
                _files.TryGetValue(fileName, out result);
                return result.CancellationTokenSource;
            }
            catch (ArgumentNullException)
            {
                // log
                throw;
            }
        }

        public StatusOfDownload GetStatus(string fileName)
        {
            try
            {
                DownloadFileInfo result;
                _files.TryGetValue(fileName, out result);
                return result.StatusOfDownload;
            }
            catch (ArgumentNullException)
            {
                // log
                throw;
            }
        }
        public int Count()
        {
            return _files.Count;
        }

        public void SaveCurrentState(string path)
        {
            Serializer.Serialize(_files, path);
        }

        public void ReadFromFile(string path)
        {
            _files = Serializer.Deserialize<ConcurrentDictionary<string, DownloadFileInfo>>(path);
        }
    }
}
