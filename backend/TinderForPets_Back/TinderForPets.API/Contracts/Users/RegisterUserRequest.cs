using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Users
{
    public record RegisterUserRequest(
        [Required] string UserName, 
        [Required] string Password,
        [Required][EmailAddress(ErrorMessage = "Invalid email address format")] string Email
    );
}
