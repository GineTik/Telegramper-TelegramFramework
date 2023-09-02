namespace Telegramper.Storage.Dictionary
{
    public interface IDictionaryStorage<TKey, TValue>
    {
        IDictionary<TKey, TValue> Items { get; }
    }
}
