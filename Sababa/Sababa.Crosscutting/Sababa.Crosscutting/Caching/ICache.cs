namespace Sababa.Crosscutting.Caching
{
    public interface ICache
    {
        /// <summary>
        /// Inserts a cache item into the cache
        /// </summary>
        /// <typeparam name="T">The type of inserting item</typeparam>
        /// <param name="item">The value to insert</param>
        void AddItem<T>(string key, T item) where T : class;

        /// <summary>
        /// Remove a cache item from the cache
        /// </summary>
        void RemoveItem(string key);

        /// <summary>
        /// Determins whether a cache item exists in the cache
        /// </summary>
        bool IsExistKey(string key);

        /// <summary>
        /// Returns a cache item from the cache
        /// </summary>
        /// <typeparam name="T">The type of a cache item</typeparam>
        /// <returns></returns>
        T GetItem<T>(string key) where T : class;
    }
}