using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IBreedRepository
    {
        Task<List<Breed>> GetBreedsAsync(CancellationToken cancellationToken);
        Task<List<Breed>> GetBreedsByTypeIdAsync(int animalTypeId, CancellationToken cancellationToken);

    }
}
