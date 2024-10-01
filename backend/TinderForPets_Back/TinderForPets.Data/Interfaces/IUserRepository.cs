using TinderForPets.Core.Models;
using SharedKernel;

namespace TinderForPets.Data.Interfaces
{
    public interface IUserRepository
    {   
        Task<Guid> Add(User user);
        Task<Result<User>> GetByEmail(string email); 
    }
}