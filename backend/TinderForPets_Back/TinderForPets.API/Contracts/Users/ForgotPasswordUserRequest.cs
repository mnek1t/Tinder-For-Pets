using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Users
{
    public record ForgotPasswordUserRequest([Required]string Email);
}
