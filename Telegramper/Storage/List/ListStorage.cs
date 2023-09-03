namespace Telegramper.Storage.List
{
    public class ListStorage<TItem> : IListStorage<TItem>
    {
        public ICollection<TItem> Items { get; }
        
        public ListStorage(ICollection<TItem> items)
        {
            Items = items;
        }
    }
}
