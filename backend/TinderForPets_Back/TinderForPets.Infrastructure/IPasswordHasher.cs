using TinderForPets.Infrastructure;
using TinderForPets.Core.Models;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Infrastructure
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Verify(string password, string hashedPassword);
    }
}
