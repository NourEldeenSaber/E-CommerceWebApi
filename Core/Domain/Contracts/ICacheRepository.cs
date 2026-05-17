namespace Domain.Contracts
{
    public interface ICacheRepository
    {
        // Get => Cached [Return Data]
        Task<string?> GetAsync(string key);

        // Set => No Cache Happend [ Apply Caching ]
        Task SetAsync(string key, object value, TimeSpan duration);

    }
}
