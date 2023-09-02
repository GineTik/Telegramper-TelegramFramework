namespace Telegramper.Storage.List
{
    public interface IListStorage<TItem>
    {
        IEnumerable<TItem> Items { get; }
    }
}
