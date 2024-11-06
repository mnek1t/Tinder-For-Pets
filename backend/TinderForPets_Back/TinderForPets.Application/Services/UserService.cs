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
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
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
                    return Result.Success<string>(userId.ToString());
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
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var userEntity = await _userRepository.GetByEmailAsync(email, cancellationToken);
                if (!userEntity.EmailConfirmed) 
                {
                    return Result.Failure<string>(UserErrors.NotFoundByEmail(email));
                }

                var userModel = _mapper.Map<User>(userEntity);
                if (!_passwordHasher.Verify(password, userModel.PasswordHash))
                {
                    return Result.Failure<string>(UserErrors.InvalidPassword);
                }

                var jwtTokenResult = GenerateAuthToken(userEntity.Id);
                return jwtTokenResult;
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

        public async Task<Result<string>> ConfirmAccount(string confirmAccountToken , CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var verifyTokenResult = _jwtProvider.ValidateConfirmAccountToken(confirmAccountToken);
                if (verifyTokenResult.IsFailure)
                {
                    return verifyTokenResult;
                }

                var userId = Guid.Parse(verifyTokenResult.Value);
                await _userRepository.ConfirmAccount(userId, cancellationToken);
                return Result.Success<string>(userId.ToString());
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<string>(new Error("400", "Operation canceled"));
            }

        }

        public async Task<Result<string>> ResetPassword(string newPassword, string confirmPassword, string resetPasswordToken, CancellationToken cancellationToken) 
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (newPassword != confirmPassword)
                {
                    return Result.Failure<string>(UserErrors.NotMatchPassword);
                }

                var verifyTokenResult = _jwtProvider.ValidateResetPasswordToken(resetPasswordToken);
                if (verifyTokenResult.IsFailure)
                {
                    return verifyTokenResult;
                }

                var hashedPassword = _passwordHasher.Generate(newPassword);
                var userEmail = verifyTokenResult.Value;
                var result = await _userRepository.ResetPassword(userEmail, hashedPassword, cancellationToken);
                return Result.Success<string>(result);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<string>(new Error("400", "Operation canceled"));
            }
            catch (UserNotFoundException ex)
            {
                return Result.Failure<string>(UserErrors.NotFoundByEmail(ex.Message));
            }
        }

        public async Task<Result<User>> FindUser(string email, CancellationToken cancellationToken) 
        {    
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var userEntity = await _userRepository.GetByEmailAsync(email, cancellationToken);
                if (!userEntity.EmailConfirmed)
                {
                    return Result.Failure<User>(UserErrors.NotFoundByEmail(email));
                }

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
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
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
        public Result<string> GenerateAuthToken(Guid userId)
        {
            var token = _jwtProvider.GenerateToken(userId);
            return Result.Success<string>(token);
        }

        public Result<string> GenerateResetPasswordToken(string email)
        {
            var token = _jwtProvider.GenerateResetPasswordToken(email);
            return Result.Success<string>(token); 
        }

        public Result<string> GenerateConfirmAccountToken(string email)
        {
            var token = _jwtProvider.GenerateConfirmAccountToken(email);
            return Result.Success<string>(token);
        }
    }

}