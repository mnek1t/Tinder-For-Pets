using SharedKernel;
using TinderForPets.Core.Models;
namespace TinderForPets.Data.Interfaces
{
    public interface IAnimalProfileRepository
    {
        Task<Result<Guid>> CreateProfile(AnimalProfile animalProfile);
        void DeleteProfile();
        void UpdateProfile();

    }
}
