using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Text.Json;
using TinderForPets.API.Contracts.Users;
using TinderForPets.API.DTOs;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;

namespace TinderForPets.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IEmailSender _emailSender;
        public UserController(UserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }

        // TODO: DELETE: For testing Autentification
        [Authorize]
        [HttpGet("protected")]
        public IActionResult GetProtectedData()
        {
            return Ok("This is a protected endpoint");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IResult> Register(
            [FromBody]RegisterUserRequest request,
            CancellationToken token)
        {
            var result =  await _userService.Register(request.UserName, request.Email, request.Password);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IResult> Login(
            [FromBody] LoginUserRequest request,
            CancellationToken token)
        {
            var result = await _userService.Login(request.Email, request.Password);
            if (result.IsFailure) 
            {
                return result.ToProblemDetails();
            }

            var jwtToken = result.Value;

            HttpContext.Response.Cookies.Append("AuthToken", jwtToken);

            return Results.Ok(jwtToken);
            
        }

        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public async Task<IResult> ResetPassword(
            [FromBody] ResetPasswordUserRequest request,
            CancellationToken token)
        {
            var result = await _userService.ResetPassword(request.NewPassword, request.ConfirmPassword, request.Token);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IResult> ForgotPassword(
            [FromBody] ForgotPasswordUserRequest request,
            CancellationToken token)
        {
            var result = await _userService.FindUser(request.Email);

            if (result.IsFailure) 
            {
                return result.ToProblemDetails();
            }
            var user = result.Value;
            var jwtToken = _userService.GenerateResetPasswordToken(request.Email).Value;

            // TODO: here will be a link to frontend and then fronend will use this link below
            var emailData = new ResetPasswordEmailDto
            {
                UserName = user.UserName,
                ResetLink = $"https://localhost:5295/api/user/resetPassword?token={jwtToken}"
            };
            var message = JsonSerializer.Serialize(emailData);

            await _emailSender.SendEmailAsync(request.Email, "Tinder For Pets Password Reset", message);

            return Results.Ok(jwtToken);
        }
    }
}
