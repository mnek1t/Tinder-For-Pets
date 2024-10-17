using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IBreedRepository
    {
        Task<List<Breed>> GetBreedsAsync();
        Task<List<Breed>?> GetBreedsByTypeIdAsync(int animalTypeId);

    }
}
