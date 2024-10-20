using TinderForPets.Core.Models;

namespace TinderForPets.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> Add(User user);
        Task<string> Delete(Guid userId);
        Task<User> GetByEmail(string email);
        Task<string> ResetPassword(string email, string hashedPassword);

    }
}