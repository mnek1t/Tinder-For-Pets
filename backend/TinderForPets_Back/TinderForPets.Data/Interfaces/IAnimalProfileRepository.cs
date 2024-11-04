using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalProfileRepository : IRepository<AnimalProfile>
    {
        Task<AnimalProfile> GetByAnimalIdAsync(Guid animalId, CancellationToken cancellationToken);
        Task<Guid> GetAnimalProfileByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);
    }
}
