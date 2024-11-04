using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IUserRepository : IRepository<UserAccount>
    {
        Task<UserAccount> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<string> ResetPassword(string email, string hashedPassword, CancellationToken cancellationToken);

    }
}