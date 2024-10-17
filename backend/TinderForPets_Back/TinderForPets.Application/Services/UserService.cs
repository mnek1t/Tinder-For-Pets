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
            var result = await _userRepository.GetByEmail(email);
            if (result.IsSuccess) 
            {
                return Result.Failure<Guid>(UserErrors.DuplicateUser(email));
            }

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

        public async Task<Result<string>> ResetPassword(string newPassword, string confirmPassword, string token) 
        {
            if (newPassword != confirmPassword) 
            {
                return Result.Failure<string>(UserErrors.NotMatchPassword);
            }

            var varifyTokenResult = _jwtProvider.ValidateResetPasswordToken(token);
            if (varifyTokenResult.IsFailure) 
            {
                return varifyTokenResult;
            }
            string hashedPassword = _passwordHasher.Generate(newPassword);
            var userEmail = varifyTokenResult.Value;
            var result = await _userRepository.ResetPassword(userEmail, hashedPassword);
            return result;
        }

        public async Task<Result<User>> FindUser(string email) 
        {
            var result = await _userRepository.GetByEmail(email);
            return result;
        }
        public Result<string> GenerateResetPasswordToken(string email)
        {
            var token = _jwtProvider.GenerateResetPasswordToken(email);
            return Result.Success<string>(token); 
        }
    }

}