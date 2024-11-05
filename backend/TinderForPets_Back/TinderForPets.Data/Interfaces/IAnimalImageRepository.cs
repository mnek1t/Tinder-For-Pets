using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalImageRepository : IRepository<AnimalImage>
    {
        Task<AnimalImage> GetAnimalImageAsync(Guid animalProfileId, CancellationToken cancellationToken);
    }
}
