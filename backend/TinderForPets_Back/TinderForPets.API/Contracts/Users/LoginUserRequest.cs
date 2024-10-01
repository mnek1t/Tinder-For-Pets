using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Users
{
    public record LoginUserRequest(
        [Required] [EmailAddress(ErrorMessage = "Invalid format")]string Email, 
        [Required] string Password);
}
