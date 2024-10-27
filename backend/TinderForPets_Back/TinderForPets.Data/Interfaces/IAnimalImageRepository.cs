using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalImageRepository
    {
        Task<List<AnimalImage>> SaveAnimalMediaAsync(IEnumerable<AnimalImageModel> animalMedia);
    }
}
