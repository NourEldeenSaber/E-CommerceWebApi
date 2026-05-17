using Domain.Contracts;
using Sevices.Abstraction.Contracts;

namespace Services.Implementations
{
    public class CacheService(ICacheRepository _cacheRepository) : ICacheService
    {
        public async Task<string?> GetChachedValueAsync(string key)
        => await _cacheRepository.GetAsync(key);

        public async Task SetCacheValueAsync(string key, object value, TimeSpan duration)
        => await _cacheRepository.SetAsync(key, value, duration);
    }
}
