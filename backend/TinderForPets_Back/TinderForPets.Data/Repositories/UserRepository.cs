using TinderForPets.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;

namespace TinderForPets.Data.Repositories
{
    public class UserRepository : TinderForPetsRepository<UserAccount>, IUserRepository
    {
        public UserRepository(TinderForPetsDbContext context) : base(context) { }

        public async override Task<Guid> CreateAsync(UserAccount user, CancellationToken cancellationToken)
        {
            await _context.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async override Task  DeleteAsync(Guid id, CancellationToken cancellationToken) 
        {
            var rowsDeleted = await _context.UserAccounts.Where(u => u.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            if (rowsDeleted == 0) 
            {
                throw new UserNotFoundException();
            }
        }

        public async override Task<UserAccount> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var userEntity = await _context.UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (userEntity == null)
            {
                throw new UserNotFoundException(id.ToString());
            }

            return userEntity;
        }

        public async Task<UserAccount> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var userEntity = await _context.UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.EmailAddress == email, cancellationToken);
            if (userEntity == null) 
            {
                throw new UserNotFoundException(email);
            }

            return userEntity;
        }

        public async Task<string> ResetPassword(string email, string hashedPassword, CancellationToken cancellationToken)
        {
            var rowsUpdated = await _context.UserAccounts
               .Where(u => u.EmailAddress == email)
               .ExecuteUpdateAsync(u => u
                   .SetProperty(u => u.Password, hashedPassword), cancellationToken);

            if (rowsUpdated == 0) 
            {
                throw new UserNotFoundException(email);
            }

            return "Password was reset";
        }

        public override Task UpdateAsync(UserAccount account, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}