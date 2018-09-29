using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sababa.Logic.SelfCuncurrentDictionary
{
    public class SyncDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISyncDictionary<TKey, TValue>
    {
        private readonly object _lock = new object();
        private readonly Dictionary<TKey, TValue> _dictionary;

        public SyncDictionary(int capacity = 0)
        {
            if (capacity == 0)
            {
                _dictionary = new Dictionary<TKey, TValue>();
            }
            else
            {
                _dictionary = new Dictionary<TKey, TValue>(capacity);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary[key];
                }
            }

            set
            {
                lock (_lock)
                {
                    _dictionary[key] = value;
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary.Keys;
                }
            } 
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary.Values;
                }
            }
        }

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException("Incoming parameter the key wasn`t initialized.");

            lock (_lock)
            {
                _dictionary.Add(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null) throw new ArgumentNullException("Incoming parameter the item wasn`t initialized.");

            lock (_lock)
            {
                _dictionary.Add(item.Key, item.Value);
            }
        }

        public void Clear() => _dictionary.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        public bool Remove(TKey key)
        {
            if (key == null) throw new ArgumentNullException("Incoming parameter the key wasn`t initialized.");

            lock (_lock)
            {
                return _dictionary.Remove(key);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (keyValuePair.Key == null) throw new ArgumentNullException("Incoming parameter the keyValuePair wasn`t initialized.");
            if (!ContainsKey(keyValuePair.Key)) return false;
            if (_dictionary.TryGetValue(keyValuePair.Key, out TValue value))
            {
                if (EqualityComparer<TValue>.Default.Equals(value, keyValuePair.Value))
                {
                    return Remove(keyValuePair.Key);
                }
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            _dictionary.TryGetValue(key, out value);
            if (value == null)
            {
                return false;
            }

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool TryAdd(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException("Incoming parameter the key wasn`t initialized.");

            lock (_lock)
            {
                if (ContainsKey(key)) return false;
                _dictionary[key] = value;
            }
            return true;
        }

        public TValue GetOrAdd(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException("Incoming parameter the key wasn`t initialized.");

            lock (_lock)
            {
                if (ContainsKey(key))
                {
                    return _dictionary[key];
                }
                else
                {
                    _dictionary[key] = value;
                    return _dictionary[key];
                }
            }
        }

        public bool TryUpdate(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException("Incoming parameter the key wasn`t initialized.");

            lock (_lock)
            {
                if (ContainsKey(key))
                {
                    _dictionary[key] = value;
                    return true;
                }

                return false;
            }
        }

        public bool TryRemove(TKey key)
        {
            if (key == null) throw new ArgumentNullException("Incoming parameter the key wasn`t initialized.");

            lock (_lock)
            {
                return _dictionary.Remove(key);
            }
        }

        public bool ContainsValue(TValue value) => _dictionary.ContainsValue(value);

    }
}
