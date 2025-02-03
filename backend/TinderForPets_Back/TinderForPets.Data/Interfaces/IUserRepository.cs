using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IUserRepository : IRepository<UserAccount>
    {
        Task<UserAccount> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task ResetPassword(string email, string hashedPassword, CancellationToken cancellationToken);
        Task ConfirmAccount(Guid id, CancellationToken cancellationToken);

    }
}