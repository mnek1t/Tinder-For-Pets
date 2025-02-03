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

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken) 
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
                throw new UserNotFoundException();
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
                throw new UserNotFoundException();
            }

            return userEntity;
        }

        public async Task ResetPassword(string email, string hashedPassword, CancellationToken cancellationToken)
        {
            var rowsUpdated = await _context.UserAccounts
               .Where(u => u.EmailAddress == email)
               .ExecuteUpdateAsync(u => u
                   .SetProperty(u => u.Password, hashedPassword), cancellationToken);

            if (rowsUpdated == 0) 
            {
                throw new UserNotFoundException();
            }
        }
        public async Task ConfirmAccount(Guid id, CancellationToken cancellationToken)
        {
            var rowsUpdated = await _context.UserAccounts
               .Where(u => u.Id == id)
               .ExecuteUpdateAsync(u => u
                   .SetProperty(u => u.EmailConfirmed, true), cancellationToken);

            if (rowsUpdated == 0)
            {
                throw new UserNotFoundException();
            }
        }
        public override async Task UpdateAsync(UserAccount user, CancellationToken cancellationToken)
        {
            var rowsUpdated = await _context.UserAccounts
               .Where(u => u.Id == user.Id)
               .ExecuteUpdateAsync(u => u
                   .SetProperty(u => u.UserName, u => u.UserName == null ? u.UserName : user.UserName)
                   .SetProperty(u => u.EmailAddress, u => u.EmailAddress == null ? u.EmailAddress : user.EmailAddress),
                   cancellationToken); 

            if (rowsUpdated == 0)
            {
                throw new UserNotFoundException();
            }
        }
    }
}