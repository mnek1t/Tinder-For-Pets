using AutoMapper;
using FluentAssertions;
using Moq;
using SharedKernel;
using TinderForPets.Application.Services;
using TinderForPets.Core;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
using TinderForPets.Data.Interfaces;
using TinderForPets.Infrastructure;

namespace TinderForPets.UnitTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IJwtProvider> _jwtProviderMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;
        
        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _jwtProviderMock = new Mock<IJwtProvider>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtProviderMock.Object, 
                _mapperMock.Object
            );

        }

        #region Register
        [Fact]
        public async Task RegisterUser_ReturnsOperationIsCancelledFailure()
        {
            // arrange
            var cts = new CancellationTokenSource();
            cts.Cancel();

            //act
            var result = await _userService.Register("TestUserName", "test@example.com", "password", cts.Token);
            cts.Dispose();

            //assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("400");
            result.Error.Description.Should().Be("Operation canceled");
        }

        [Fact]
        public async Task RegisterUser_ReturnsDuplicationUserFailure()
        {
            var email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserAccount());

            var result = await _userService.Register("TestUserName", email, "password", CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Users.DuplicateUser");
            result.Error.Description.Should().Be($"The user with {email} email already exists");
        }

        [Fact]
        public async Task RegisterUser_ReturnsUserCreationFailure()
        {
            var email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<UserAccount>(), CancellationToken.None))
                .ThrowsAsync(new UserNotFoundException());

            var result = await _userService.Register("TestUserName", email, "password", CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(UserErrors.NotCreated.Code);
            result.Error.Description.Should().Be(UserErrors.NotCreated.Description);
        }

        [Fact]
        public async Task RegisterUser_ReturnsUserCreationSuccess()
        {
            var email = "test@example.com";
            var password = "password123";
            var hashedPassword = "hashedPassword";
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            _passwordHasherMock.Setup(hasher => hasher.Generate(password))
                .Returns(hashedPassword);

            _mapperMock.Setup(mapper => mapper.Map<UserAccount>(It.IsAny<User>()))
                .Returns(new UserAccount { Id = userId });

            _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<UserAccount>(), CancellationToken.None))
                .ReturnsAsync(userId);

            var result = await _userService.Register("TestUserName", email, password, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(userId.ToString());
        }
        #endregion

        #region Login
        [Fact]
        public async Task LoginUser_ReturnsOperationIsCancelledFailure() 
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var result = await _userService.Login("TestUser", "password", cts.Token);
            cts.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("400");
            result.Error.Description.Should().Be("Operation canceled");
        }

        [Fact]
        public async Task LoginUser_ReturnsUserNotFoundFailure()
        {
            var email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            var result = await _userService.Login(email, "password", CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Users.NotFoundByEmail");
            result.Error.Description.Should().Be("The user does not exist");
        }

        [Fact]
        public async Task LoginUser_ReturnsEmailIsNotConfirmedYetFailure()
        {
            var email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserAccount() { EmailConfirmed  = false});

            var result = await _userService.Login(email, "password", CancellationToken.None);
            result.IsFailure.Should().BeTrue();

            result.Error.Code.Should().Be("Users.NotFoundByEmail");
            result.Error.Description.Should().Be("The user does not exist");
        }

        [Fact]
        public async Task LoginUser_ReturnsInvalidPasswordFailure()
        {
            var email = "test@example.com";
            var invalidPassword = "invalidPassword";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserAccount() { EmailConfirmed = true });

            _mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserAccount>()))
                .Returns(new User());

            _passwordHasherMock.Setup(hasher => hasher.Verify(invalidPassword, "hashedPassword"))
                .Returns(false);

            var result = await _userService.Login(email, "password", CancellationToken.None);
            result.IsFailure.Should().BeTrue();

            result.Error.Code.Should().Be(UserErrors.InvalidPassword.Code);
            result.Error.Description.Should().Be(UserErrors.InvalidPassword.Description);
        }

        [Fact]
        public async Task LoginUser_ReturnsUserSuccess()
        {
            var email = "test@example.com";
            var userId = Guid.NewGuid();
            var correctPassword = "correctPassword";
            var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(correctPassword);
            var mockJwt = "jwttoken";
            var userEntity = new UserAccount
            {
                Id = userId,
                EmailAddress = email,
                EmailConfirmed = true,
                Password = hashedPassword
            };

            var userModel = User.Create(userEntity.Id, "test", userEntity.Password, userEntity.EmailAddress);

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userEntity);

            _mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserAccount>()))
                .Returns(userModel);

            _passwordHasherMock.Setup(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            _jwtProviderMock.Setup(provider => provider.GenerateToken(userId))
                .Returns(mockJwt);

            var result = await _userService.Login(email, correctPassword, CancellationToken.None);
            
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(mockJwt);

            _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()), Times.Once);
            _passwordHasherMock.Verify(hasher => hasher.Verify(correctPassword, userModel.PasswordHash), Times.Once);
            _jwtProviderMock.Verify(jwt => jwt.GenerateToken(userId), Times.Once);
        }
        #endregion

        #region FindUser
        [Fact]
        public async Task FindUser_ReturnsOperationCanceledFailure() 
        {
            var cst = new CancellationTokenSource();
            cst.Cancel();

            var result = await _userService.FindUser("test@example.com", cst.Token);

            cst.Dispose();
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("400");
            result.Error.Description.Should().Be("Operation canceled");
        }

        [Fact]
        public async Task FindUser_ReturnsUserNotFoundFailure()
        {
            var email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, CancellationToken.None))
                .ThrowsAsync(new UserNotFoundException());

            var result = await _userService.FindUser(email, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Users.NotFoundByEmail");
            result.Error.Description.Should().Be("The user does not exist");
        }

        [Fact]
        public async Task FindUser_ReturnsEmailIsNotConfirmedYetFailure()
        {
            var email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserAccount() { EmailConfirmed = false });

            var result = await _userService.FindUser(email, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Users.NotFoundByEmail");
            result.Error.Description.Should().Be("The user does not exist");
        }

        [Fact]
        public async Task FindUser_ReturnsUserSuccess()
        {
            var email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserAccount() { EmailConfirmed = true });
            
            _mapperMock.Setup(repo => repo.Map<User>(It.IsAny<UserAccount>()))
                .Returns(new User());

            var result = await _userService.FindUser(email, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteUser_ReturnsOperationCanceledFailure()
        {
            var userId = Guid.NewGuid();
            var cst = new CancellationTokenSource();
            cst.Cancel();

            var result = await _userService.DeleteUser(userId, cst.Token);

            cst.Dispose();
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("400");
            result.Error.Description.Should().Be("Operation canceled");
        }

        [Fact]
        public async Task DeleteUser_ReturnsUserNotFoundFailure()
        {
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.DeleteAsync(userId, CancellationToken.None))
                .ThrowsAsync(new UserNotFoundException());

            var result = await _userService.DeleteUser(userId, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("Users.NotFoundById");
            result.Error.Description.Should().Be($"The user with the Id = {userId} was not found");
        }

        [Fact]
        public async Task DeleteUser_ReturnsDeleteSuccess()
        {
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.DeleteAsync(userId, CancellationToken.None))
                .Returns(Task.CompletedTask);

            var result = await _userService.DeleteUser(userId, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            _userRepositoryMock.Verify(repo => repo.DeleteAsync(userId, CancellationToken.None), Times.Once);
        }
        #endregion

        #region ConfirmAccount
        [Fact]
        public async Task ConfirmAccount_ReturnsOperationCanceledFailure()
        {
            var confirmAccountToken = "confirmAccountToken";
            var cst = new CancellationTokenSource();
            cst.Cancel();

            var result = await _userService.ConfirmAccount(confirmAccountToken, cst.Token);

            cst.Dispose();
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("400");
            result.Error.Description.Should().Be("Operation canceled");
        }

        [Fact]
        public async Task ConfirmAccount_ReturnsInvalidTokenFailure()
        {
            var confirmAccountToken = "confirmAccountToken";
            var invalidTokenError = JwtErrors.InvalidToken;
            _jwtProviderMock.Setup(provider => provider.ValidateConfirmAccountToken(confirmAccountToken))
                .Returns(Result.Failure<string>(invalidTokenError));

            var result = await _userService.ConfirmAccount(confirmAccountToken, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(JwtErrors.InvalidToken.Code);
            result.Error.Description.Should().Be(JwtErrors.InvalidToken.Description);
        }

        [Fact]
        public async Task ConfirmAccount_ReturnsExpiredTokenFailure()
        {
            var confirmAccountToken = "confirmAccountToken";
            var expiredTokenError = JwtErrors.JwtTokenExpired;

            _jwtProviderMock.Setup(provider => provider.ValidateConfirmAccountToken(confirmAccountToken))
                .Returns(Result.Failure<string>(expiredTokenError));

            var result = await _userService.ConfirmAccount(confirmAccountToken, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(JwtErrors.JwtTokenExpired.Code);
            result.Error.Description.Should().Be(JwtErrors.JwtTokenExpired.Description);
        }

        [Fact]
        public async Task ConfirmAccount_ReturnsUserNotFoundFailure()
        {
            var userId = Guid.NewGuid();
            var confirmAccountToken = "confirmAccountToken";

            _jwtProviderMock.Setup(provider => provider.ValidateConfirmAccountToken(confirmAccountToken))
                .Returns(Result.Success<string>(userId.ToString()));
            _userRepositoryMock.Setup(repo => repo.ConfirmAccount(userId, CancellationToken.None))
                .ThrowsAsync(new UserNotFoundException());

            var result = await _userService.ConfirmAccount(confirmAccountToken, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(UserErrors.NotFound.Code);
            result.Error.Description.Should().Be(UserErrors.NotFound.Description);
        }

        [Fact]
        public async Task ConfirmAccount_ReturnsSuccess()
        {
            var userId = Guid.NewGuid();
            var confirmAccountToken = "confirmAccountToken";

            _jwtProviderMock.Setup(provider => provider.ValidateConfirmAccountToken(confirmAccountToken))
                .Returns(Result.Success<string>(userId.ToString()));

            _userRepositoryMock.Setup(repo => repo.ConfirmAccount(userId, CancellationToken.None))
                .Returns(Task.CompletedTask);

            var result = await _userService.ConfirmAccount(confirmAccountToken, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(userId.ToString());
        }
        #endregion

        #region ResetPassword
        [Fact]
        public async Task ResetPassword_ReturnsOperationCanceledFailure()
        {
            var resetPasswordToken = "resetPasswordToken";
            var cst = new CancellationTokenSource();
            cst.Cancel();

            var result = await _userService.ResetPassword("password", "password", resetPasswordToken, cst.Token);
            cst.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("400");
            result.Error.Description.Should().Be("Operation canceled");
        }

        [Fact]
        public async Task ResetPassword_ReturnsPasswordDoesNotMatchFailure()
        {
            var resetPasswordToken = "resetPasswordToken";
            var newPassword = "newPassword";
            var confirmPassword = "confirmPassword";

            var result = await _userService.ResetPassword(newPassword, confirmPassword, resetPasswordToken, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(UserErrors.NotMatchPassword.Code);
            result.Error.Description.Should().Be(UserErrors.NotMatchPassword.Description);
        }

        [Fact]
        public async Task ResetPassword_ReturnsInvalidTokenFailure()
        {
            var resetPasswordToken = "resetPasswordToken";
            var newPassword = "newPassword";
            var confirmPassword = "newPassword";
            var invalidToken = JwtErrors.InvalidToken;

            _jwtProviderMock.Setup(provider => provider.ValidateResetPasswordToken(resetPasswordToken))
               .Returns(Result.Failure<string>(invalidToken));

            var result = await _userService.ResetPassword(newPassword, confirmPassword, resetPasswordToken, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(JwtErrors.InvalidToken.Code);
            result.Error.Description.Should().Be(JwtErrors.InvalidToken.Description);
        }

        [Fact]
        public async Task ResetPassword_ReturnsExpiredTokenFailure()
        {
            var resetPasswordToken = "resetPasswordToken";
            var newPassword = "newPassword";
            var confirmPassword = "newPassword";
            var expiredTokenError = JwtErrors.JwtTokenExpired;

            _jwtProviderMock.Setup(provider => provider.ValidateResetPasswordToken(resetPasswordToken))
               .Returns(Result.Failure<string>(expiredTokenError));

            var result = await _userService.ResetPassword(newPassword, confirmPassword, resetPasswordToken, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(JwtErrors.JwtTokenExpired.Code);
            result.Error.Description.Should().Be(JwtErrors.JwtTokenExpired.Description);
        }

        [Fact]
        public async Task ResetPassword_ReturnsUserNotFoundFailure()
        {
            var resetPasswordToken = "resetPasswordToken";
            var email = "test@example.com";
            var newPassword = "newPassword";
            var confirmPassword = "newPassword";
            
            _jwtProviderMock.Setup(provider => provider.ValidateResetPasswordToken(resetPasswordToken))
                .Returns(Result.Success<string>(email));

            _userRepositoryMock.Setup(repo => repo.ResetPassword(email, It.IsAny<string>(), CancellationToken.None))
                .ThrowsAsync(new UserNotFoundException());

            var result = await _userService.ResetPassword(newPassword, confirmPassword, resetPasswordToken, CancellationToken.None);


            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(UserErrors.NotFound.Code);
            result.Error.Description.Should().Be(UserErrors.NotFound.Description);
        }

        [Fact]
        public async Task ResetPassword_ReturnsSuccess()
        {
            var resetPasswordToken = "resetPasswordToken";
            var email = "test@example.com";
            var newPassword = "newPassword";
            var confirmPassword = "newPassword";

            _jwtProviderMock.Setup(provider => provider.ValidateResetPasswordToken(resetPasswordToken))
                .Returns(Result.Success<string>(email));

            _userRepositoryMock.Setup(repo => repo.ResetPassword(email, confirmPassword, CancellationToken.None))
                .Returns(Task.CompletedTask);

            var result = await _userService.ResetPassword(newPassword, confirmPassword, resetPasswordToken, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(email);
        }
        #endregion
    }
}