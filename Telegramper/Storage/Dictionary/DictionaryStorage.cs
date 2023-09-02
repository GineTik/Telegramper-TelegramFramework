namespace Telegramper.Storage.Dictionary
{
    public class DictionaryStorage<TKey, TValue> : IDictionaryStorage<TKey, TValue>
        where TKey : notnull
    {
        public DictionaryStorage(IDictionary<TKey, TValue> items)
        {
            Items = items;
        }

        public IDictionary<TKey, TValue> Items { get; }
    }
}
