using Sababa.Data.Storage.Interfaces;

namespace Sababa.Data.Storage.Classes
{
    public class StorageFactory
    {
        /// <summary>
        /// Returns an instance of class DocumentStorage.
        /// </summary>
        /// <returns>Instance of class DocumentStorage.</returns>
        public static IKeyValueStorage<string, Document> GetKeyValueStorage()
        {
            return StorageHolder.KeyValueStorage;
        }

        public static IKeyValueStorage<string, Document> GetSecureStorage()
        {
            return StorageHolder.SecureStorage;
        }

        public static IFileStorage GetFileStorage(bool fileCollectorIsWorking = false,
            int delayFileCollector = 15000, int periodfFleCollector = 30000)
        {
            return StorageHolder.GetFileStorage(fileCollectorIsWorking, delayFileCollector, periodfFleCollector);
        }

        private static class StorageHolder
        {
            private static IFileStorage _fileStorage;

            public static IFileStorage GetFileStorage(bool fileCollectorIsWorking = false, int delayFileCollector = 0, int periodfFleCollector = 0)
            {
                return _fileStorage = new FileStorage(fileCollectorIsWorking, delayFileCollector, periodfFleCollector);
            }

            public static IKeyValueStorage<string, Document> KeyValueStorage { get; } = new KeyValueStorage();

            public static IKeyValueStorage<string, Document> SecureStorage { get; } = new SecureStorage(null);
        }
    }
}
