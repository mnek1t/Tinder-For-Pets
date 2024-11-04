using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Profiles
{
    public record AnimalMediaUploadRequest([Required] Guid UserId, [Required] Guid AnimalProfileId, string Description, [Required] IFormFile File);
}
