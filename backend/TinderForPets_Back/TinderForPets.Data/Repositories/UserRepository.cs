using TinderForPets.Data.Interfaces;
using TinderForPets.Core.Models;
using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;
using SharedKernel;
using TinderForPets.Core;
using AutoMapper;

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
            var userEntity = _mapper.Map<UserAccount>(user);

            await _context.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.Id;
        }

        public async Task Update(User user)
        {
            // TODO: complete 
            if (user == null) 
            {
                throw new Exception("User is null");
            }

            await _context.UserAccounts
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.EmailAddress, user.Email)
                    .SetProperty(u => u.UserName, user.UserName)
                    .SetProperty(u => u.Password, user.PasswordHash));
        }

        public async Task<Result<User>> GetByEmail(string email)
        {
            var userEntity = await _context.UserAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.EmailAddress == email);
            if (userEntity == null) 
            {
                return Result.Failure<User>(UserErrors.NotFoundByEmail(email));
            }

            var user = _mapper.Map<User>(userEntity);

            return Result.Success<User>(user);
        }

        public async Task<List<User>> GetUsers()
        {
            // TODO: complete 
            var userEntities = await _context.UserAccounts.AsNoTracking().ToListAsync();

            // TODO: Create User type defined in Core from Entity. Use Mapper!   

            throw new NotImplementedException();
        }
    }
}