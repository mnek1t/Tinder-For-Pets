using SharedKernel;
using TinderForPets.Core.Models;
namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalProfileRepository
    {
        Task<Guid> CreateAnimalAsync(AnimalModel animalModel);
        Task<Guid> CreateProfileAsync(AnimalProfileModel animalProfile);
        void DeleteProfile();
        void UpdateProfile();

    }
}
