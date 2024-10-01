using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using TinderForPets.API.Contracts.Users;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;

namespace TinderForPets.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
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
            return Results.Ok(result.Value);
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
    }
}
