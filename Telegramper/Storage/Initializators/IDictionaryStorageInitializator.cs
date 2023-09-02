namespace Telegramper.Storage.Initializators
{
    public interface IDictionaryStorageInitializator<TKey, TValue>
    {
        IDictionary<TKey, TValue> Initialization();
    }
}
