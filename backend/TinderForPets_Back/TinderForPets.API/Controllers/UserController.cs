using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TinderForPets.API.Contracts.Users;
using TinderForPets.API.DTOs;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;
using TinderForPets.Core.Models;
using TinderForPets.Infrastructure;

namespace TinderForPets.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly IJwtProvider _jwtProvider;
        public UserController(UserService userService, IEmailSender emailSender, IJwtProvider jwtProvider)
        {
            _userService = userService;
            _emailSender = emailSender;
            _jwtProvider = jwtProvider;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IResult> Register(
            [FromBody] RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _userService.Register(request.UserName, request.Email, request.Password, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            var confirmAccountTokenResult = _userService.GenerateConfirmAccountToken(result.Value);
            if (confirmAccountTokenResult.IsFailure)
            {
                return confirmAccountTokenResult.ToProblemDetails();
            }
            var confirmAccountToken = confirmAccountTokenResult.Value;
            var emailData = new ConfirmAccountDto
            {
                EmailAddress = request.Email,
                //TODO: replace by frontend link
                ConfirmAccountLink = $"https://localhost:3000/accounts/confirm?token={confirmAccountToken}"
            };
            var message = JsonSerializer.Serialize(emailData);
            try
            {
                await _emailSender.SendEmailAsync(request.Email, "Tinder For Pets Confirm Account", message);
            }
            catch (Exception ex)
            {
                return Results.Problem(statusCode: 500, title: ex.Message);
            }

            return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
        }

        [HttpPatch("account/confirm")]
        [AllowAnonymous]
        public async Task<IResult> ConfirmAccount(
            [FromBody] ConfirmAccountRequest request,
            CancellationToken cancellationToken)
        {
            var confirmAccountResult = await _userService.ConfirmAccount(request.Token, cancellationToken);
            if (confirmAccountResult.IsFailure)
            {
                return confirmAccountResult.ToProblemDetails();
            }
            var userId = Guid.Parse(confirmAccountResult.Value);
            var jwtTokenResult = _userService.GenerateAuthToken(userId);
            if (jwtTokenResult.IsFailure)
            {
                return jwtTokenResult.ToProblemDetails();
            }

            var jwtToken = jwtTokenResult.Value;
            SetAuthTokenInCookies(HttpContext, jwtToken);
            return confirmAccountResult.IsSuccess ? Results.NoContent() : confirmAccountResult.ToProblemDetails();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IResult> Login(
            [FromBody] LoginUserRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _userService.Login(request.Email, request.Password, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            var jwtToken = result.Value;
            SetAuthTokenInCookies(HttpContext, jwtToken);

            return Results.Ok(jwtToken);
        }

        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<IResult> GoogleLogin([FromBody] GoogleLoginRequest request, CancellationToken cancellationToken)  
        {
            var googleAuthOptions = HttpContext.RequestServices.GetRequiredService<IOptions<GoogleAuthOptions>>().Value;

            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { googleAuthOptions.ClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleToken, settings);
            var result = await _userService.FindUser(payload.Email, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            var userId = result.Value.Id;
            var jwtToken = _jwtProvider.GenerateToken(userId);
            SetAuthTokenInCookies(HttpContext, jwtToken);       
            return Results.Ok(jwtToken);
        }

        [Authorize]
        [HttpPost("logout")]
        public IResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return Results.Ok("You have been logged out");
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IResult> DeleteAccount(CancellationToken cancellationToken)
        {
            var validationTokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (validationTokenResult.IsFailure)
            {
                return validationTokenResult.ToProblemDetails();
            }
            var userId = validationTokenResult.Value;
            Logout();
            var result = await _userService.DeleteUser(userId, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        }

        [HttpPatch("password/reset")]
        [AllowAnonymous]
        public async Task<IResult> ResetPassword(
            [FromBody] ResetPasswordUserRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _userService.ResetPassword(request.NewPassword, request.ConfirmPassword, request.Token, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpPost("password/forgot")]
        [AllowAnonymous]
        public async Task<IResult> ForgotPassword(
            [FromBody] ForgotPasswordUserRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _userService.FindUser(request.Email, cancellationToken);

            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            var user = result.Value;
            var resetPasswordToken = _userService.GenerateResetPasswordToken(request.Email).Value;

            var emailData = new ResetPasswordEmailDto
            {
                UserName = user.UserName,
                ResetLink = $"https://localhost:3000/accounts/password/reset?token={resetPasswordToken}"
            };
            var message = JsonSerializer.Serialize(emailData);

            await _emailSender.SendEmailAsync(request.Email, "Tinder For Pets Password Reset", message);

            return Results.Ok(resetPasswordToken);
        }

        private static void SetAuthTokenInCookies(HttpContext context, string jwtToken) 
        {
            context.Response.Cookies.Append("AuthToken", jwtToken, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
    }
}
