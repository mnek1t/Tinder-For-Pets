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
        private readonly JsonSerializerOptions _serializerOptions;

        public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer redis)
        {
            _cache = cache;
            _redis = redis;
            _serializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T?> GetAsync<T>(string cacheKey)
        {
            var cachedData = await _cache.GetAsync(cacheKey);
            if (cachedData == null)
            {
                return default;
            }
            try
            {
                return JsonSerializer.Deserialize<T>(cachedData, _serializerOptions);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Failed to deserialize cache data for key {cacheKey}.", ex);
            }
            
        }

        public async Task RemoveAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task SetAsync<T>(string cacheKey, T value, TimeSpan expirationTime)
        {
            var serializedData = JsonSerializer.Serialize(value, _serializerOptions);
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
                    if (Guid.TryParse(swipedAnimalProfileId, out var guid))
                    {
                        listData.Add(guid);
                    }
                }
            }
            return listData;
        }
    }
}
