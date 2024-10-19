using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Profiles
{
    public record UpdateAnimalProfileRequest(
        [Required] string Name,
        [Required] int TypeId,
            string Description,
        [Required] int Age,
        [Required] int SexId,
        [Required] bool IsVaccinated,
        [Required] bool IsSterilized,
        [Required] int BreedId
        );
}
