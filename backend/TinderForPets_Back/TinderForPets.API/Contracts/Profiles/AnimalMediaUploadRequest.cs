using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Profiles
{
    public record AnimalMediaUploadRequest(string Description, [Required] IFormFile File);
}
