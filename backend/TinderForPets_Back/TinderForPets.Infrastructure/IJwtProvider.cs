using TinderForPets.Core.Models;

namespace TinderForPets.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}