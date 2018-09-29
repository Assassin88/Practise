using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sababa.Crosscutting.Logging;
using Sababa.Data.Storage.Interfaces;
using static System.String;

namespace Sababa.Data.Storage.Classes
{
    public class FileStorage : IFileStorage
    {
        private string _storageDir;
        private readonly object _lock = new object();
        private readonly List<string> _fileStorage = new List<string>();
        private Timer _fileCollector;
        private readonly BaseLogger _baseLogger = new BaseLogger("FileStorageLogger");

        internal FileStorage(bool fileCollectorIsWorking = false, int delayFileCollector = 0, int periodfFleCollector = 0)
        {
            if (fileCollectorIsWorking)
            {
                _fileCollector = new Timer(FileInspector, null, delayFileCollector, periodfFleCollector);
            }

            LoadConfig();
        }

        #region Methods
        private void LoadConfig()
        {
            _storageDir = StorageSettings.GetPathDirectory("FileStorage");

            if (!StorageSettings.CreateDirectoryIfNotExists(_storageDir)) return;
            foreach (var item in GetFiles())
            {
                lock (_lock)
                {
                    _fileStorage.Add(Path.GetFileName(item));
                }
            }
        }

        private void FileInspector(object state)
        {
            _baseLogger.LogTrace("Check lifetime files");
            foreach (var item in GetFiles())
            {
                if (IsNullOrEmpty(item)) continue;

                try
                {
                    if (new FileInfo(item).LastAccessTime < DateTime.Now)
                    {
                        File.Delete(item);
                        _baseLogger.LogTrace($"File '{item}' deleted.");
                        lock (_lock)
                        {
                            _fileStorage.Remove(Path.GetFileName(item));
                        }
                    }
                }
                catch (ArgumentException)
                {
                    _baseLogger.LogError($"File '{item}' has an invalid name.");
                }
                catch (IOException ex)
                {
                    _baseLogger.LogError($"The following exception '{ex}' occurred while checking file lifetimes.");
                }
                catch (Exception ex)
                {
                    _baseLogger.LogFatal($"The application is terminated due to an exception '{ex}'.");
                    throw;
                }
            }
        }

        private bool VerifyFileName(string fileName)
        {
            return !IsNullOrEmpty(fileName);
        }

        public bool Save(Stream stream, string fileName, DateTime lifeTime)
        {
            if (!VerifyFileName(fileName))
            {
                _baseLogger.LogTrace("The file was not saved because the file name was specified incorrectly.");
                return false;
            }

            try
            {
                _baseLogger.LogTrace($"Create a stream to write the value to a file '{fileName}'.");
                using (var file = new FileStream(GetFullPath(fileName), FileMode.Create))
                {
                    stream.CopyTo(file);
                }

                lock (_lock)
                {
                    _fileStorage.Add(fileName);
                }

                File.SetLastAccessTime(GetFullPath(fileName), lifeTime);
            }
            catch (ArgumentException)
            {
                _baseLogger.LogError($"File '{fileName}' has an invalid name.");
            }
            catch (IOException ex)
            {
                _baseLogger.LogError($"The following exception '{ex}' occurred while writing the value to the file.");
            }
            catch (Exception ex)
            {
                _baseLogger.LogFatal($"The application is terminated due to an exception '{ex}'.");
                throw;
            }

            return true;
        }

        public async Task<bool> SaveAsync(Stream stream, string fileName, DateTime lifeTime)
        {
            if (!VerifyFileName(fileName))
            {
                _baseLogger.LogTrace("The file was not saved because the file name was specified incorrectly.");
                return false;
            }

            try
            {
                _baseLogger.LogTrace($"Create a stream to write the value to a file '{fileName}'.");
                using (var file = new FileStream(GetFullPath(fileName), FileMode.Create))
                {
                    await stream.CopyToAsync(file);
                }

                lock (_lock)
                {
                    _fileStorage.Add(fileName);
                }

                File.SetLastAccessTime(GetFullPath(fileName), lifeTime);
            }
            catch (ArgumentException)
            {
                _baseLogger.LogError($"File '{fileName}' has an invalid name.");
            }
            catch (IOException ex)
            {
                _baseLogger.LogError($"The following exception '{ex}' occurred while writing the value to the file.");
            }
            catch (Exception ex)
            {
                _baseLogger.LogFatal($"The application is terminated due to an exception '{ex}'.");
                throw;
            }

            return true;
        }

        public bool Save(byte[] bytes, string fileName, DateTime lifeTime)
        {
            if (!VerifyFileName(fileName))
            {
                _baseLogger.LogTrace("The file was not saved because the file name was specified incorrectly.");
                return false;
            }

            try
            {
                _baseLogger.LogTrace($"Create a stream to write the value to a file '{fileName}'.");

                using (var stream = new FileStream(GetFullPath(fileName), FileMode.Create, FileAccess.Write))
                {
                    if (stream.CanWrite)
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }

                lock (_lock)
                {
                    _fileStorage.Add(fileName);
                }

                File.SetLastAccessTime(GetFullPath(fileName), lifeTime);
            }
            catch (ArgumentException)
            {
                _baseLogger.LogError($"File '{fileName}' has an invalid name.");
            }
            catch (IOException ex)
            {
                _baseLogger.LogError($"The following exception '{ex}' occurred while writing the value to the file.");
            }
            catch (Exception ex)
            {
                _baseLogger.LogFatal($"The application is terminated due to an exception '{ex}'.");
                throw;
            }

            return true;
        }

        public async Task<bool> SaveAsync(byte[] bytes, string fileName, DateTime lifeTime)
        {
            if (!VerifyFileName(fileName))
            {
                _baseLogger.LogTrace("The file was not saved because the file name was specified incorrectly.");
                return false;
            }

            try
            {
                _baseLogger.LogTrace($"Create a stream to write the value to a file '{fileName}'.");

                using (var stream = new FileStream(GetFullPath(fileName), FileMode.Create, FileAccess.Write))
                {
                    if (stream.CanWrite)
                    {
                        await stream.WriteAsync(bytes, 0, bytes.Length);
                    }
                }

                lock (_lock)
                {
                    _fileStorage.Add(fileName);
                }

                File.SetLastAccessTime(GetFullPath(fileName), lifeTime);
            }
            catch (ArgumentException)
            {
                _baseLogger.LogError($"File '{fileName}' has an invalid name.");
            }
            catch (IOException ex)
            {
                _baseLogger.LogError($"The following exception '{ex}' occurred while writing the value to the file.");
            }
            catch (Exception ex)
            {
                _baseLogger.LogFatal($"The application is terminated due to an exception '{ex}'.");
                throw;
            }

            return true;
        }

        public byte[] ReadAllBytes(string fileName)
        {
            if (!VerifyFileName(fileName))
            {
                _baseLogger.LogTrace("The file was not readed because the file name was specified incorrectly.");
                return new byte[0];
            }

            try
            {
                _baseLogger.LogTrace($"Create a stream to read the value from the file '{fileName}'.");
                using (var stream = new FileStream(GetFullPath(fileName), FileMode.Open, FileAccess.Read))
                {
                    if (stream.CanRead)
                    {
                        var bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        return bytes;
                    }
                }
            }
            catch (ArgumentException)
            {
                _baseLogger.LogError($"File '{fileName}' has an invalid name.");
            }
            catch (IOException ex)
            {
                _baseLogger.LogError($"The following exception '{ex}' occurred while reading the value to the file.");
            }
            catch (Exception ex)
            {
                _baseLogger.LogFatal($"The application is terminated due to an exception '{ex}'.");
                throw;
            }

            return new byte[0];
        }

        public async Task<byte[]> ReadAllBytesAsync(string fileName)
        {
            if (!VerifyFileName(fileName))
            {
                _baseLogger.LogTrace("The file was not readed because the file name was specified incorrectly.");
                return new byte[0];
            }

            try
            {
                _baseLogger.LogTrace($"Create a stream to read the value from the file '{fileName}'.");
                using (var stream = new FileStream(GetFullPath(fileName), FileMode.Open, FileAccess.Read))
                {
                    if (stream.CanRead)
                    {
                        var bytes = new byte[stream.Length];
                        await stream.ReadAsync(bytes, 0, bytes.Length);
                        return bytes;
                    }
                }
            }
            catch (ArgumentException)
            {
                _baseLogger.LogError($"File '{fileName}' has an invalid name.");
            }
            catch (IOException ex)
            {
                _baseLogger.LogError($"The following exception '{ex}' occurred while reading the value to the file.");
            }
            catch (Exception ex)
            {
                _baseLogger.LogFatal($"The application is terminated due to an exception '{ex}'.");
                throw;
            }

            return new byte[0];
        }

        public Stream GetStream(string fileName)
        {
            return new FileStream(GetFullPath(fileName), FileMode.Open, FileAccess.Read);
        }

        private List<string> GetFiles()
        {
            return Directory.GetFiles(_storageDir).ToList();
        }

        private string GetFullPath(string fileName)
        {
            return IsNullOrEmpty(fileName) ? $"{_storageDir}defaultFile" : $"{_storageDir}{fileName}";
        }

        public bool Remove(string fileName)
        {
            lock (_lock)
            {
                if (!_fileStorage.Contains(fileName))
                {
                    _baseLogger.LogTrace($"File '{fileName}' not found.");
                    return false;
                }
                _baseLogger.LogTrace($"Delete file '{fileName}'.");
                _fileStorage.Remove(fileName);

                try
                {
                    File.Delete(GetFullPath(fileName));
                }
                catch (IOException ex)
                {
                    _baseLogger.LogError($"Could not completely remove file '{fileName}' due to exception '{ex}'.");
                }
                catch (Exception ex)
                {
                    _baseLogger.LogFatal($"The following exception '{ex}' occurred while trying to remove file '{fileName}'.");
                    throw;
                }

                return true;
            }
        }

        public void Clear()
        {
            _baseLogger.LogTrace("Clear file storage.");

            try
            {
                _fileStorage.Clear();

                foreach (var item in GetFiles())
                {
                    File.Delete(item);
                }
            }
            catch (IOException ex)
            {
                _baseLogger.LogError($"Could not completely clear storage due to exception '{ex}'.");
            }
            catch (Exception ex)
            {
                _baseLogger.LogFatal($"The following exception '{ex}' occurred while trying to clear storage.");
                throw;
            }
        }

        public int Count()
        {
            return _fileStorage.Count;
        }
        #endregion
    }
}

