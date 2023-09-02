namespace Telegramper.Storage.Initializators
{
    public interface IListStorageInitializator<TItem>
    {
        IEnumerable<TItem> Initialization();
    }
}
