namespace Telegramper.Storage.List
{
    public interface IListStorage<TItem>
    {
        ICollection<TItem> Items { get; }
    }
}
