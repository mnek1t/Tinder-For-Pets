using TinderForPets.Infrastructure;
using TinderForPets.Core.Models;
using TinderForPets.Data.Interfaces;
using SharedKernel;
using TinderForPets.Core;
using TinderForPets.Data.Exceptions;

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
            try
            {
                await _userRepository.GetByEmail(email);
                return Result.Failure<Guid>(UserErrors.DuplicateUser(email));
            }
            catch (UserNotFoundException) 
            { }

            var hashedPassword = _passwordHasher.Generate(password);
            var user = User.Create(Guid.NewGuid(), userName, hashedPassword, email);
            try
            {
                var userId = await _userRepository.Add(user);
                return Result.Success<Guid>(userId);
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<Guid>(UserErrors.NotCreated);
            }
        }

        public async Task<Result<string>> Login(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetByEmail(email);

                if (!_passwordHasher.Verify(password, user.PasswordHash))
                {
                    return Result.Failure<string>(UserErrors.InvalidPassword);
                }
                var token = _jwtProvider.GenerateToken(user);
                return Result.Success<string>(token);
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<string>(UserErrors.NotFoundByEmail(email));
            }
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
            
            try
            {
                var result = await _userRepository.ResetPassword(userEmail, hashedPassword);
                return Result.Success<string>(result);
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<string>(UserErrors.NotFoundByEmail(userEmail));
            }
        }

        public async Task<Result<User>> FindUser(string email) 
        {
            try
            {
                var user = await _userRepository.GetByEmail(email);
                return Result.Success<User>(user);
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<User>(UserErrors.NotFoundByEmail(email));
            } 
        }

        public Result<string> GenerateResetPasswordToken(string email)
        {
            var token = _jwtProvider.GenerateResetPasswordToken(email);
            return Result.Success<string>(token); 
        }
    }

}