using Microsoft.AspNetCore.Http;
using SharedKernel;
namespace TinderForPets.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(Guid userId);
        string GenerateResetPasswordToken(string email);
        string GenerateConfirmAccountToken(string userId);

        Result<string> ValidateResetPasswordToken(string token);
        Result<string> ValidateConfirmAccountToken(string token);
        Result<Guid> ValidateAuthTokenAndExtractUserId(HttpContext context);
    }
}