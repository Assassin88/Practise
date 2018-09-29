namespace Sababa.Logic.SelfCuncurrentDictionary
{
    public interface ISyncDictionary<TKey, TValue>
    {
        bool ContainsValue(TValue value);
        TValue GetOrAdd(TKey key, TValue value);
        bool TryAdd(TKey key, TValue value);
        bool TryRemove(TKey key);
        bool TryUpdate(TKey key, TValue value);
    }
}