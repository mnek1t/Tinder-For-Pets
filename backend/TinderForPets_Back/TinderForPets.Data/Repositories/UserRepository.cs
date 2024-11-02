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
        private readonly IMapper _mapper;

        public UserRepository(TinderForPetsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Add(User user)
        {
            if (user == null) 
            {
                throw new UserNotFoundException();
            }

            var userEntity = _mapper.Map<UserAccount>(user);

            await _context.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.Id;
        }

        public async Task<string> Delete(Guid userId) 
        {
            var rowsDeleted = await _context.UserAccounts.Where(u => u.Id == userId)
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();

            return rowsDeleted == 0 ? throw new UserNotFoundException() : "Success. User Has been deleted and his pets as well";
        }

        public async Task<User> GetByEmail(string email)
        {
            var userEntity = await _context.UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.EmailAddress == email);
            if (userEntity == null) 
            {
                throw new UserNotFoundException(email);
            }

            var user = _mapper.Map<User>(userEntity);

            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            // TODO: complete 
            var userEntities = await _context.UserAccounts.AsNoTracking().ToListAsync();

            // TODO: Create User type defined in Core from Entity. Use Mapper!   

            throw new NotImplementedException();
        }

        public async Task<string> ResetPassword(string email, string hashedPassword)
        {
            var rowsUpdated = await _context.UserAccounts
               .Where(u => u.EmailAddress == email)
               .ExecuteUpdateAsync(u => u
                   .SetProperty(u => u.Password, hashedPassword));
            await _context.SaveChangesAsync();

            return rowsUpdated == 0 ? throw new UserNotFoundException(email) : "Password was reset";
        }
    }
}