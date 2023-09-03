using System.Collections;

namespace Telegramper.Storage.Dictionary
{
    public class DictionaryStorage<TKey, TValue> : IDictionaryStorage<TKey, TValue>
        where TKey : notnull
    {
        public IDictionary<TKey, TValue> Items { get; }

        public DictionaryStorage(IDictionary<TKey, TValue> items)
        {
            Items = items;
        }
    }

    public class DictionaryStorage<TDictionary> : IDictionaryStorage<TDictionary>
        where TDictionary : IDictionary
    {
        public TDictionary Items { get; }

        public DictionaryStorage(TDictionary items)
        {
            Items = items;
        }
    }
}
