using System.Collections;

namespace Telegramper.Storage.Initializers
{
    public interface IDictionaryStorageInitializer<TKey, TValue>
    {
        IDictionary<TKey, TValue> Initialization();
    }

    public interface IDictionaryStorageInitializer<TDictionary>
        where TDictionary : IDictionary
    {
        TDictionary Initialization();
    }
}
