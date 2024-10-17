using SharedKernel;
namespace TinderForPets.Core
{
    public class JwtErrors
    {
        public static readonly Error JwtTokenExpired = new(
            "JwtToken.Expired", "Provided JWT Token expired");

        public static readonly Error InvalidToken = new(
            "JwtToken.InvalidToken", "Invalid Token");
    }
}
