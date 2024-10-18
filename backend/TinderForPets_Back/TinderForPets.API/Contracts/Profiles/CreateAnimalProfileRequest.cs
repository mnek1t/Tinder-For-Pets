using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Profiles
{
    public record CreateAnimalProfileRequest(
        [Required] string Name,
        [Required] int TypeId,
            string Description,
        [Required] int Age,
        [Required] int SexId,
        [Required] bool IsVaccinated,
        [Required] bool IsSterilized,
        [Required] int BreedId,
        [Required] Guid OwnerId // in the top of my head, we need to take this from cookie where jwt token is located. jwt token includes encrypted userId
        );
}
