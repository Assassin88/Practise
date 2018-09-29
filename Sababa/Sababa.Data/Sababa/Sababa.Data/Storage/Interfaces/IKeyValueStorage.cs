using System.Threading.Tasks;

namespace Sababa.Data.Storage.Interfaces
{
    public interface IKeyValueStorage<TKey, TValue>
    {
        /// <summary>
        /// Reads a value using a unique key
        /// </summary>
        /// <param name="key">The key of the element to search</param>
        /// <returns></returns>
        TValue FindOne(TKey key);

        Task<TValue> FindOneAsync(TKey key);

        /// <summary>
        /// Saves or updates a value using a unique key
        /// </summary>
        /// <param name="key">The key of the element to add or update</param>
        /// <param name="value">The value of the element to add or update</param>
        /// <returns></returns>
        void SaveOrUpdate(TKey key, TValue value);

        Task SaveOrUpdateAsync(TKey key, TValue value);

        /// <summary>
        /// Gets the number of key/value pairs contained in local storage
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Determines whether the contains in local storage the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the local storage</param>
        /// <returns></returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Removes all keys and values from the local storage
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets a value that indicates whether the local storage is empty.
        /// </summary>
        /// <returns></returns>
        bool IsEmpty();

        /// <summary>
        /// Attempts to remove value that has the specified key
        /// </summary>
        /// <param name="key">The key of the element to remove</param>
        void Remove(TKey key);

        /// <summary>
        /// Initializes the data store from the directory.
        /// </summary>
        void Initialize();
    }
}
