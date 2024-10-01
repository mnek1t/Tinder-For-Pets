using TinderForPets.Infrastructure;
using TinderForPets.Core.Models;
using TinderForPets.Data.Interfaces;
using SharedKernel;
using TinderForPets.Core;

namespace TinderForPets.Application.Services
{
    public class UserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public UserService(
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<Guid>> Register(string userName, string email, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);
            var user = User.Create(Guid.NewGuid(), userName, hashedPassword, email);

            return await _userRepository.Add(user);
        }

        public async Task<Result<string>> Login(string email, string password)
        {
            var result = await _userRepository.GetByEmail(email);

            if (result.IsFailure)
            {
                return Result.Failure<string>(result.Error);
            }

            var user = result.Value;

            if (!_passwordHasher.Verify(password, user.PasswordHash))
            {
                return Result.Failure<string>(UserErrors.InvalidPassword); 
            }

            var token = _jwtProvider.GenerateToken(user);

            return Result.Success<string>(token);
        }
    }
}