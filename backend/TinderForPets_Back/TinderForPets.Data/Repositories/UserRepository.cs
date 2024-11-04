using TinderForPets.Data.Interfaces;
using TinderForPets.Core.Models;
using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;
using TinderForPets.Core;
using AutoMapper;
using TinderForPets.Data.Exceptions;

namespace TinderForPets.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TinderForPetsDbContext _context;

        public UserRepository(TinderForPetsDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(UserAccount user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task DeleteAsync(Guid id) 
        {
            var rowsDeleted = await _context.UserAccounts.Where(u => u.Id == id)
                .ExecuteDeleteAsync();
            if (rowsDeleted == 0) 
            {
                throw new UserNotFoundException();
            }
        }

        public async Task<UserAccount> GetByIdAsync(Guid id)
        {
            var userEntity = await _context.UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

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

        public async Task<string> ResetPassword(string email, string hashedPassword)
        {
            var rowsUpdated = await _context.UserAccounts
               .Where(u => u.EmailAddress == email)
               .ExecuteUpdateAsync(u => u
                   .SetProperty(u => u.Password, hashedPassword));

            if (rowsUpdated == 0) 
            {
                throw new UserNotFoundException(email);
            }

            return "Password was reset";
        }

        public Task UpdateAsync(UserAccount account)
        {
            throw new NotImplementedException();
        }
    }
}