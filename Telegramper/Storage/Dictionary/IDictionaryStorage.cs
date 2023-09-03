using System.Collections;

namespace Telegramper.Storage.Dictionary
{
    public interface IDictionaryStorage<TKey, TValue>
    {
        IDictionary<TKey, TValue> Items { get; }
    }

    public interface IDictionaryStorage<TDictionary>
        where TDictionary : IDictionary
    {
        TDictionary Items { get; }
    }
}
