namespace Telegramper.Storage.Initializers
{
    public interface IDictionaryStorageInitializer<TKey, TValue>
    {
        IDictionary<TKey, TValue> Initialization();
    }
}
