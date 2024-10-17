using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TinderForPets.Application.DTOs;
using TinderForPets.Application.Interfaces;

namespace TinderForPets.Application.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
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
    }
}
