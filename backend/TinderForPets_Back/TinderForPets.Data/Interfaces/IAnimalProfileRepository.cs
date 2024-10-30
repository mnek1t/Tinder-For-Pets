using TinderForPets.Core.Models;
namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalProfileRepository
    {
        Task<Guid> CreateAnimalAsync(AnimalModel animalModel);
        Task<Guid> CreateProfileAsync(AnimalProfileModel animalProfile);
        void DeleteProfile();
        Task<int> UpdateAnimalAsync(AnimalModel animalModel);
        Task<int> UpdateProfileAsync(AnimalProfileModel animalProfileModel);
    }
}
