using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        Task<Guid> CreateAsync(T entity, CancellationToken cancellationToken);
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    }
}
