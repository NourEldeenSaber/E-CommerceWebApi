namespace Sevices.Abstraction.Contracts
{
    public interface ICacheService
    {
        Task<string?> GetChachedValueAsync(string key);
        Task SetCacheValueAsync(string key, object value, TimeSpan duration);
    }
}
