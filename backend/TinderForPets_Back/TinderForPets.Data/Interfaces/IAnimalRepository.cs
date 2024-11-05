using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalRepository : IRepository<Animal>
    {
        Task<Animal> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
