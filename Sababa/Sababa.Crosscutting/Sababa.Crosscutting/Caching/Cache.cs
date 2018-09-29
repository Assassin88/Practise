using System.Runtime.Caching;

namespace Sababa.Crosscutting.Caching
{
    public class Cache: ICache
    {
        private readonly MemoryCache _memoryCache;

        /// <summary>
        /// Initialize a new instaince of the Cache class
        /// </summary>
        public Cache()
        {
            _memoryCache = MemoryCache.Default;
        }

        public void AddItem<T>(string key, T item) where T : class
        {
            _memoryCache.Set(key, item, null);
        }

        public T GetItem<T>(string key) where T : class
        {
            return _memoryCache.Get(key) as T;
        }

        public bool IsExistKey(string key)
        {
            return _memoryCache.Contains(key);
        }

        public void RemoveItem(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
