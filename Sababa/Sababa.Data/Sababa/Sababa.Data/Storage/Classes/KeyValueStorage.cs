using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sababa.Crosscutting.Logging;
using Sababa.Data.Storage.Interfaces;

namespace Sababa.Data.Storage.Classes
{
    public class KeyValueStorage : IKeyValueStorage<string, Document>
    {
        #region Fields and properties
        private string _storageDir = StorageSettings.GetPathDirectory(null);
        private readonly string _fileExtension = ".txt";
        private ConcurrentDictionary<string, Document> _documents = new ConcurrentDictionary<string, Document>();
        private readonly BaseLogger _baseLogger = new BaseLogger("KeyValueStorageLogger");

        public string StorageDirectory { get => _storageDir; set => _storageDir = value; }

        public BaseLogger Logger => _baseLogger;

        public ConcurrentDictionary<string, Document> Documents
        {
            get => _documents;
            protected set => _documents = value;
        }
        #endregion

        internal KeyValueStorage()
        {
            if (!StorageSettings.CreateDirectoryIfNotExists(StorageDirectory))
                _baseLogger.LogTrace("Create new directory.");
        }

        #region Methods
        public void Initialize()
        {
            Array.ForEach(Directory.GetFiles(_storageDir, $"*{_fileExtension}"),
                f => FindOne(Path.GetFileNameWithoutExtension(f)));
        }

        public virtual Document FindOne(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _baseLogger.LogTrace("Returns empty document because key is null or empty.");
                return new EmptyDocument();
            }

            if (_documents.TryGetValue(key, out var document))
            {
                _baseLogger.LogTrace($"Returns value '{document}' by key '{key}' from memory.");
                return document;
            }

            try
            {
                _baseLogger.LogTrace($"Returns value by key '{key}' from file.");
                return _documents.GetOrAdd(key, ReadFromFile);
            }
            catch (OverflowException)
            {
                _baseLogger.LogError($"The dictionary overflow occurred, the value by key {key} will be written to the new object.");
                _documents = new ConcurrentDictionary<string, Document>();
                return _documents.GetOrAdd(key, ReadFromFile);
            }
        }

        public virtual async Task<Document> FindOneAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _baseLogger.LogTrace("Returns empty document because key is null or empty.");
                return new EmptyDocument();
            }

            if (_documents.TryGetValue(key, out var document))
            {
                _baseLogger.LogTrace($"Returns value '{document}' by key '{key}' from memory.");
                return document;
            }

            try
            {
                _baseLogger.LogTrace($"Returns value by key '{key}' from file.");
                return _documents.GetOrAdd(key, ReadFromFile);
            }
            catch (OverflowException)
            {
                _baseLogger.LogError($"The dictionary overflow occurred, the value by key {key} will be written to the new object.");
                _documents = new ConcurrentDictionary<string, Document>();
                return _documents.GetOrAdd(key, await ReadFromFileAsync(key));
            }
        }

        /// <summary>
        /// Reads a document from the repository along the specified path
        /// </summary>
        /// <param name="key">The key of the element to read</param>
        /// <returns>Returns a read document or a empty document</returns>
        protected Document ReadFromFile(string key)
        {
            try
            {
                _baseLogger.LogTrace($"Reads value from file by key '{key}'.");
                var value = File.ReadAllText(GetFullPath(key));
                return JsonConvert.DeserializeObject<Document>(value, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            }
            catch (Exception ex)
            {
                _baseLogger.LogError($"An exception '{ex}' occurred while reading the value from the file. Return an empty document.");
                return new EmptyDocument();
            }
        }

        /// <summary>
        /// Reads a document from the repository along the specified path asynchronously
        /// </summary>
        /// <param name="key">The key of the element to read</param>
        /// <returns>Return task</returns>
        protected async Task<Document> ReadFromFileAsync(string key)
        {
            try
            {
                _baseLogger.LogTrace($"Reads value from file by key '{key}'.");
                var value = string.Empty;
                using (var reader = new StreamReader(GetFullPath(key)))
                {
                    value = await reader.ReadToEndAsync();
                }
                return JsonConvert.DeserializeObject<Document>(value, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            }
            catch (Exception ex)
            {
                _baseLogger.LogError($"An exception '{ex}' occurred while reading the value from the file. Return an empty document.");
                return new EmptyDocument();
            }
        }

        public virtual void SaveOrUpdate(string key, Document value)
        {
            if (string.IsNullOrEmpty(key))
            {
                _baseLogger.LogTrace("Returns from method because key is null or empty.");
                return;
            }
            try
            {
                _baseLogger.LogTrace($"Write value '{value}' by key '{key}' to memory.");
                _documents[key] = value;

                _baseLogger.LogTrace($"Called a method to write a value '{value}' by key '{key}' to a file.");
                WriteToFile(key, value);
            }
            catch (KeyNotFoundException)
            {
                _baseLogger.LogError($"This key '{key}' does not exist in collection.");
            }
            catch (IOException ex)
            {
                _baseLogger.LogError($"An exception '{ex}' occurred while saving the value to a file.");
            }
            catch (Exception ex)
            {
                _baseLogger.LogFatal($"The application terminated exception '{ex}'.");
                throw;
            }
        }

        public virtual async Task SaveOrUpdateAsync(string key, Document value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    _baseLogger.LogTrace($"Write value '{value}' by key '{key}' to memory.");
                    _documents[key] = value;

                    _baseLogger.LogTrace($"Called a method to write a value '{value}' by key '{key}' to a file.");
                    await WriteToFileAsync(key, value);
                }
                catch (KeyNotFoundException)
                {
                    _baseLogger.LogError($"This key '{key}' does not exist in collection.");
                }
                catch (IOException ex)
                {
                    _baseLogger.LogError($"An exception '{ex}' occurred while saving the value to a file.");
                }
                catch (Exception ex)
                {
                    _baseLogger.LogFatal($"The application terminated exception '{ex}'.");
                    throw;
                }
            }
        }

        /// <summary>
        /// Stores the document in serialized form asynchronously
        /// </summary>
        /// <param name="key">The key of the element to write</param>
        /// <param name="value">The value of the element to write</param>
        /// <returns>Returns task</returns>
        protected async Task WriteToFileAsync(string key, Document value)
        {
            var result = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });

            using (var stream = new StreamWriter(GetFullPath(key)))
            {
                await stream.WriteAsync(result);
            }

            _baseLogger.LogTrace($"Wrote a value '{value}' by name '{key}' to the file.");
        }

        /// <summary>
        /// Stores the document in serialized form 
        /// </summary>
        /// <param name="key">The key of the element to write</param>
        /// <param name="value">The value of the element to write</param>
        protected void WriteToFile(string key, Document value)
        {
            File.WriteAllText(GetFullPath(key),
                JsonConvert.SerializeObject(value, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                }));

            _baseLogger.LogTrace($"Wrote a value '{value}' by name '{key}' to the file.");
        }

        public void Remove(string key)
        {
            var isDeleted = _documents.TryRemove(key, out var document);
            _baseLogger.LogTrace($"Deleted value '{document}' by key '{key}' from memory.");

            if (!isDeleted)
            {
                _baseLogger.LogTrace($"Deleted value '{document}' by key '{key}' from memory fail. Return from the method.");
                return;
            }
            try
            {
                File.Delete(GetFullPath(key));
                _baseLogger.LogTrace($"Deleted file by name '{key}'.");
            }
            catch (DirectoryNotFoundException)
            {
                _baseLogger.LogError("Сould not find the directory.");
            }
            catch (IOException ex)
            {
                _baseLogger.LogError($"The file was not deleted due to an exception '{ex}'.");
            }
            catch (Exception ex)
            {
                _baseLogger.LogFatal($"The following exception '{ex}' occurred while trying to delete the file.");
                throw;
            }
        }

        private string GetFullPath(string key)
        {
            return String.IsNullOrEmpty(key) ? $"{_storageDir}defaultFile{_fileExtension}" : $"{_storageDir}{key}{_fileExtension}";
        }

        public int Count()
        {
            return _documents.Count;
        }

        public bool ContainsKey(string key)
        {
            return !String.IsNullOrEmpty(key) && _documents.ContainsKey(key);
        }

        public void Clear()
        {
            _baseLogger.LogTrace("Clear memory and file storage.");
            _documents.Clear();
            ClearRepository();
        }

        /// <summary>
        /// Removes documents from the repository
        /// </summary>
        protected void ClearRepository()
        {
            _baseLogger.LogTrace("Clear file storage.");
            try
            {
                foreach (var item in new DirectoryInfo(_storageDir).GetFiles())
                {
                    item.Delete();
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

        public bool IsEmpty()
        {
            return _documents.IsEmpty;
        }
        #endregion
    }
}

