using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using TinderForPets.Application.Interfaces;

namespace TinderForPets.Application.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer redis)
        {
            _cache = cache;
            _redis = redis; 
        }

        public async Task<T> GetAsync<T>(string cacheKey)
        {
            var cachedData = await _cache.GetAsync(cacheKey);
            if (cachedData == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task RemoveAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task SetAsync<T>(string cacheKey, T value, TimeSpan expirationTime)
        {
            var serializedData = JsonSerializer.Serialize(value);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime,
            };
            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);
        }

        public async Task<List<Guid>> GetByPatternAsync(string cacheKeyPattern) 
        {
            var listData = new List<Guid>();
            var endpoints = _redis.GetEndPoints();
            var server = _redis.GetServer(endpoints.First());
            var keysByPattern = server.Keys(pattern: cacheKeyPattern);
            foreach (var key in keysByPattern)
            {
                if (!string.IsNullOrEmpty(key)) 
                {
                    var swipedAnimalProfileId = key.ToString().Split(':').LastOrDefault();
                    listData.Add(Guid.Parse(swipedAnimalProfileId));
                }
            }
            return listData;
        }
    }
}
