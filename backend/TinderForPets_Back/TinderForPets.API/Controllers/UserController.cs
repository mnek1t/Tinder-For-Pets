﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IResult> Register(
            [FromBody] RegisterUserRequest request,
            CancellationToken token)
        {
            var result = await _userService.Register(request.UserName, request.Email, request.Password);
            var jwtToken = result.Value;
            SetAuthTokenInCookies(HttpContext, jwtToken);

            return result.IsSuccess ? Results.Ok(jwtToken) : result.ToProblemDetails();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IResult> Login(
            [FromBody] LoginUserRequest request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _userService.Login(request.Email, request.Password, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            var jwtToken = result.Value;
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
        [HttpDelete("{id}")]
        public async Task<IResult> DeleteAccount(Guid id)
        {
            Logout();

            var result = await _userService.DeleteUser(id);
            return result.IsSuccess ? Results.Ok(result) : result.ToProblemDetails();
        }

        [HttpPatch("resetPassword")]
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

            var emailData = new ResetPasswordEmailDto
            {
                UserName = user.UserName,
                ResetLink = $"http://localhost:3000/accounts/password/reset?token={jwtToken}"
            };
            var message = JsonSerializer.Serialize(emailData);

            await _emailSender.SendEmailAsync(request.Email, "Tinder For Pets Password Reset", message);

            return Results.Ok(jwtToken);
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
