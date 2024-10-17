using System.ComponentModel.DataAnnotations;

namespace TinderForPets.API.Contracts.Users
{
    public record ResetPasswordUserRequest([Required]string NewPassword, [Required] string ConfirmPassword, [Required] string Token);
}
