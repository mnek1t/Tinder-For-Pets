using System.Linq.Expressions;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalProfileRepository : IRepository<AnimalProfile>
    {
        Task<AnimalProfile> GetByAnimalIdAsync(Guid animalId, CancellationToken cancellationToken);
        Task<AnimalProfile> GetAnimalProfileByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);
        Task<List<AnimalProfile>> GetAnimalProfilesAsync(Expression<Func<AnimalProfile, bool>> func, CancellationToken cancellationToken);
        Task<AnimalProfile> GetAnimalProfileDetails(Guid ownerId, CancellationToken cancellationToken);
    }
}
