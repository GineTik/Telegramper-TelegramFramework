namespace Telegramper.Storage.List
{
    public class ListStorage<TItem> : IListStorage<TItem>
    {
        public ListStorage(ICollection<TItem> items)
        {
            Items = items;
        }

        public ICollection<TItem> Items { get; }
    }
}
