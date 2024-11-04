using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        public Task<Guid> CreateAsync(T entity);
        public Task<T> GetByIdAsync(Guid id);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(Guid id);

    }
}
