namespace Telegramper.Storage.List
{
    public class ListStorage<TItem> : IListStorage<TItem>
    {
        public ListStorage(IEnumerable<TItem> items)
        {
            Items = items;
        }

        public IEnumerable<TItem> Items { get; }
    }
}
