using TinderForPets.Core.Models;

namespace TinderForPets.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> Add(User user);
        Task<User> GetByEmail(string email);
        Task<string> ResetPassword(string email, string hashedPassword);

    }
}