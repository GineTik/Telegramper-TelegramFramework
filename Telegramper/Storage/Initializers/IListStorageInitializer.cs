namespace Telegramper.Storage.Initializers
{
    public interface IListStorageInitializer<TItem>
    {
        IEnumerable<TItem> Initialization();
    }
}
