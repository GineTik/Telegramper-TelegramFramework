namespace Telegramper.Session
{
    public interface ISession<T>
    {
        Task<T?> GetAndRemoveAsync();
        Task<T?> GetAsync();
        Task SetAsync(T value);
        Task RemoveAsync();
    }
}
