using TinderForPets.Infrastructure;
using TinderForPets.Core.Models;
using TinderForPets.Data.Interfaces;
using SharedKernel;
using TinderForPets.Core;
using TinderForPets.Data.Exceptions;
using AutoMapper;
using TinderForPets.Data.Entities;

namespace TinderForPets.Application.Services
{
    public class UserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider,
            IMapper mapper)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        }

        public async Task<Result<string>> Register(string userName, string email, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await _userRepository.GetByEmailAsync(email, cancellationToken);
                return Result.Failure<string>(UserErrors.DuplicateUser(email));
            }
            catch (UserNotFoundException)
            {
                var hashedPassword = _passwordHasher.Generate(password);
                var user = User.Create(Guid.NewGuid(), userName, hashedPassword, email);
                try
                {
                    var userEntity = _mapper.Map<UserAccount>(user);
                    var userId = await _userRepository.CreateAsync(userEntity, cancellationToken);
                    var token = _jwtProvider.GenerateToken(userId);
                    return Result.Success<string>(token);
                }
                catch (UserNotFoundException)
                {
                    return Result.Failure<string>(UserErrors.NotCreated);
                }
                catch (OperationCanceledException)
                {
                    return Result.Failure<string>(new Error("400", "Operation canceled"));
                }
            }
            catch (OperationCanceledException) 
            {
                return Result.Failure<string>(new Error("400", "Operation canceled"));
            }
        }

        public async Task<Result<string>> Login(string email, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var userEntity = await _userRepository.GetByEmailAsync(email, cancellationToken);
                var userModel = _mapper.Map<User>(userEntity);
                if (!_passwordHasher.Verify(password, userModel.PasswordHash))
                {
                    return Result.Failure<string>(UserErrors.InvalidPassword);
                }
                var token = _jwtProvider.GenerateToken(userEntity.Id);
                return Result.Success<string>(token);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<string>(new Error("400","Operation canceled"));
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<string>(UserErrors.NotFoundByEmail(email));
            }
        }

        public async Task<Result<string>> ResetPassword(string newPassword, string confirmPassword, string resetPasswordToken, CancellationToken cancellationToken) 
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (newPassword != confirmPassword) 
            {
                return Result.Failure<string>(UserErrors.NotMatchPassword);
            }

            var varifyTokenResult = _jwtProvider.ValidateResetPasswordToken(resetPasswordToken);
            if (varifyTokenResult.IsFailure) 
            {
                return varifyTokenResult;
            }

            var hashedPassword = _passwordHasher.Generate(newPassword);
            var userEmail = varifyTokenResult.Value;
            
            try
            {
                var result = await _userRepository.ResetPassword(userEmail, hashedPassword, cancellationToken);
                return Result.Success<string>(result);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<string>(new Error("400", "Operation canceled"));
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<string>(UserErrors.NotFoundByEmail(userEmail));
            }
        }

        public async Task<Result<User>> FindUser(string email, CancellationToken cancellationToken) 
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var userEntity = await _userRepository.GetByEmailAsync(email, cancellationToken);
                var userModel = _mapper.Map<User>(userEntity);
                return Result.Success<User>(userModel);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<User>(new Error("400", "Operation canceled"));
            }
            catch (UserNotFoundException)
            {
                return Result.Failure<User>(UserErrors.NotFoundByEmail(email));
            } 
        }

        public async Task<Result> DeleteUser(Guid userId, CancellationToken cancellationToken) 
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await _userRepository.DeleteAsync(userId, cancellationToken);
                return Result.Success();
            }
            catch (UserNotFoundException)
            {
                return Result.Failure(UserErrors.NotFound(userId));
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<User>(new Error("400", "Operation canceled"));
            }
        }

        public Result<string> GenerateResetPasswordToken(string email)
        {
            var token = _jwtProvider.GenerateResetPasswordToken(email);
            return Result.Success<string>(token); 
        }
    }

}