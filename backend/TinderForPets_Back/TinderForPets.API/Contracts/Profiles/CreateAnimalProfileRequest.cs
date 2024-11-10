using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Profiles
{
    public record CreateAnimalProfileRequest(
        [Required] string Name,
        [Required] int AnimalTypeId,
            string Description,
        [Required] DateOnly DateOfBirth, 
        [Required] int SexId,
        [Required] bool IsVaccinated,
        [Required] bool IsSterilized,
        [Required] int BreedId,
        IFormFile File,
        [Required] string Country,
        [Required] string City,
        decimal Height,
        decimal Weight
        );
}
