using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderForPets.Application.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string cacheKey);
        Task SetAsync<T>(string cacheKey, T value, TimeSpan expirationTime);
        Task RemoveAsync(string cacheKey);
        Task<List<Guid>> GetByPatternAsync(string cacheKeyPattern);
    }
}
