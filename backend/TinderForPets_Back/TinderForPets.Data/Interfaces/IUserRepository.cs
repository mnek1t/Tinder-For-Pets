using TinderForPets.Core.Models;
using SharedKernel;
using System.Globalization;

namespace TinderForPets.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<Guid>> Add(User user);
        Task<Result<User>> GetByEmail(string email);
        Task<Result<string>> ResetPassword(string email, string hashedPassword);

    }
}