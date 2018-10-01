using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sababa.Logic.HttpDownloader.Classes
{
    public class DownloadClient : IDisposable, IDownloadClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private long? _streamContentLength;
        private int _bufferSize = 4 * 1024;
        public IDownloadFiles CurrentFiles { get; set; }
        public string StorageDir { get; set; } = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\DownloadFiles\\";
        public string StorageFile { get; set; }

        public DownloadClient(IDownloadFiles currentFile, string storageFile = null)
        {
            if (!Directory.Exists(StorageDir))
            {
                Directory.CreateDirectory(StorageDir);
            }
            StorageFile = storageFile ?? "JsonSaver.txt";

            this.CurrentFiles = currentFile;
            if (File.Exists(GetFullPath(StorageFile)))
            {
                CurrentFiles.ReadFromFile(GetFullPath(StorageFile));
            }
        }

        public bool AddFile(string fileName, string uri)
        {
            if (CurrentFiles.AddFile(fileName, uri))
            {
                CurrentFiles.SaveCurrentState(GetFullPath(StorageFile));
                return true;
            }

            return false;
        }

        public bool DeleteFile(string fileName)
        {
            if (CurrentFiles.DeleteFile(fileName))
            {
                CurrentFiles.SaveCurrentState(GetFullPath(StorageFile));
                return true;
            }

            return false;
        }

        private string GetFullPath(string fileName)
        {
            return $"{StorageDir}{fileName}";
        }


        public async Task<bool> StartDownload(string fileName, IProgress<DownloadArgs> progress = null)
        {
            if (CurrentFiles.GetStatus(fileName) == StatusOfDownload.Complete)
            {
                return false;
                //log
            }

            var rangeFrom = GetSizeCurrentDownloadFile(fileName);
            var response = await GetResponseAsync(rangeFrom, CurrentFiles.GetUri(fileName));
            _streamContentLength = response.Content.Headers.ContentLength;

            var downloadStream = await GetDownloadStreamAsync(response);
            await DownloadWorker(downloadStream, fileName, response.StatusCode == HttpStatusCode.PartialContent, progress);
            return true;
        }

        private Task<HttpResponseMessage> GetResponseAsync(long rangeFrom, string uri)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            httpRequestMessage.Headers.Range = new RangeHeaderValue(rangeFrom, null);
            return _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
        }

        private async Task<Stream> GetDownloadStreamAsync(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        private async Task DownloadWorker(Stream stream, string fileName, bool isPartial, IProgress<DownloadArgs> progress = null)
        {
            try
            {
                await ProcessDownloadFile(stream, fileName, isPartial, progress);
            }

            #region Exceptions

            catch (ArgumentNullException)
            {
                //Log
                //throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                //Log
                //throw;
            }
            catch (ArgumentException)
            {
                //Log
                //throw;
            }
            catch (NotSupportedException)
            {
                //Log
                //throw;
            }
            catch (ObjectDisposedException)
            {
                //Log
                //throw;
            }
            catch (InvalidOperationException)
            {
                //Log
                //throw;
            }

            #endregion
        }

        private async Task ProcessDownloadFile(Stream stream, string fileName, bool isPartial, IProgress<DownloadArgs> progress = null)
        {
            byte[] buffer = new byte[_bufferSize];
            bool IsPause = false;
            var token = CurrentFiles.GetCancellationTokenSource(fileName).Token;
            FileMode mode = isPartial ? FileMode.Append : FileMode.Create;
            using (var file = new FileStream(GetFullPath(fileName), mode))
            {
                //log
                int read, count = 0;
                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await file.WriteAsync(buffer, 0, read);
                    GetProgressInfo(progress, read);
                    if (token.IsCancellationRequested)
                    {
                        //log
                        CurrentFiles.UpdateStatus(fileName, StatusOfDownload.Pause);
                        CurrentFiles.SaveCurrentState(GetFullPath(StorageFile));
                        IsPause = true;
                        break;
                    }
                    count++;
                }
            }
            if (!IsPause)
            {
                //log
                CurrentFiles.UpdateStatus(fileName, StatusOfDownload.Complete);
                CurrentFiles.SaveCurrentState(GetFullPath(StorageFile));
            }

        }

        private void GetProgressInfo(IProgress<DownloadArgs> progress, int read)
        {
            progress?.Report(new DownloadArgs()
            {
                DownloadedByteCount = read,
                TotalBytes = _streamContentLength
            });
        }

        public void StopDownload(string fileName)
        {
            CurrentFiles.GetCancellationTokenSource(fileName).Cancel();
        }

        private long GetSizeCurrentDownloadFile(string fileName)
        {
            if (!File.Exists(GetFullPath(fileName)))
            {
                return 0;
            }

            return new FileInfo(GetFullPath(fileName)).Length;
        }


        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }

                disposedValue = true;
            }
        }

        ~DownloadClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
