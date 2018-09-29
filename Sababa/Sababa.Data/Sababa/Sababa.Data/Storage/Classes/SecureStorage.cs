using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sababa.Data.Storage.Classes
{
    public class SecureStorage : KeyValueStorage
    {
        private readonly Crypt _crypt;

        internal SecureStorage(string password, string storageDirectory = "SecureStorage")
        {
            _crypt = new Crypt(StorageSettings.GetPassword(password));

            StorageDirectory = StorageSettings.GetPathDirectory(storageDirectory);

            StorageSettings.CreateDirectoryIfNotExists(StorageDirectory);

            Initialize();
        }

        #region Methods
        public override Document FindOne(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Logger.LogTrace("Returns empty document because key is null or empty.");
                return new EmptyDocument();
            }

            if (Documents.TryGetValue(key, out var document))
            {
                Logger.LogTrace($"Returns value '{document}' by key '{key}' from memory.");
                return DecryptDocument(document);
            }

            try
            {
                Logger.LogTrace($"Returns value by key '{key}' from file.");
                document = Documents.GetOrAdd(key, ReadFromFile(key));
            }
            catch (OverflowException)
            {
                Logger.LogError($"The dictionary overflow occurred, the value by key {key} will be written to the new object.");
                Documents = new ConcurrentDictionary<string, Document>();
                document = Documents.GetOrAdd(key, ReadFromFile);
            }
           
            return DecryptDocument(document);
        }

        public override async Task<Document> FindOneAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Logger.LogTrace("Returns empty document because key is null or empty.");
                return new EmptyDocument();
            }

            if (Documents.TryGetValue(key, out var document))
            {
                Logger.LogTrace($"Returns value '{document}' by key '{key}' from memory.");
                return DecryptDocument(document);
            }

            try
            {
                Logger.LogTrace($"Returns value by key '{key}' from file.");
                document = Documents.GetOrAdd(key, ReadFromFile(key));
            }
            catch (OverflowException)
            {
                Logger.LogError($"The dictionary overflow occurred, the value by key {key} will be written to the new object.");
                Documents = new ConcurrentDictionary<string, Document>();
                document = Documents.GetOrAdd(key, await ReadFromFileAsync(key));
            }

            return DecryptDocument(document);
        }

        public override void SaveOrUpdate(string key, Document value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Logger.LogTrace("Returns from method because key is null or empty.");
                return;
            }

            try
            {
                Logger.LogTrace($"Encrypt the value '{value}'.");
                var document = EncryptDocument(value);

                Logger.LogTrace($"Write value '{value}' by key '{key}' to memory.");
                Documents[key] = document;

                Logger.LogTrace($"Called a method to write a value '{value}' by key '{key}' to a file.");
                WriteToFile(key, document);
            }
            catch (KeyNotFoundException)
            {
                Logger.LogError($"This key '{key}' does not exist in collection.");
            }
            catch (IOException ex)
            {
                Logger.LogError($"An exception '{ex}' occurred while saving the value to a file.");
            }
            catch (Exception ex)
            {
                Logger.LogFatal($"The application terminated exception '{ex}'.");
                throw;
            }

        }

        public override async Task SaveOrUpdateAsync(string key, Document value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    Logger.LogTrace($"Encrypt the value '{value}'.");
                    var document = EncryptDocument(value);

                    Logger.LogTrace($"Write value '{value}' by key '{key}' to memory.");
                    Documents[key] = document;

                    Logger.LogTrace($"Called a method to write a value '{value}' by key '{key}' to a file.");
                    await WriteToFileAsync(key, document);
                }
                catch (KeyNotFoundException)
                {
                    Logger.LogError($"This key '{key}' does not exist in collection.");
                }
                catch (IOException ex)
                {
                    Logger.LogError($"An exception '{ex}' occurred while saving the value to a file.");
                }
                catch (Exception ex)
                {
                    Logger.LogFatal($"The application terminated exception '{ex}'.");
                    throw;
                }
            }
        }

        private Document EncryptDocument(Document document)
        {
            var bytes = Encoding.Unicode.GetBytes(document.ToString());
            bytes = _crypt.Encrypt(bytes);
            var value = new Document(bytes);
            return value;
        }

        private Document DecryptDocument(Document document)
        {
            var bytes = _crypt.Decrypt((byte[])document.Value);
            var valueString = Encoding.Unicode.GetString(bytes);
            var value = new Document(JsonConvert.DeserializeObject(valueString, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            }));
            return value;
        }
        #endregion
    }
}
