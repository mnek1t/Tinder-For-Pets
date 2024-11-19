using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinderForPets.API.Contracts.Swipes;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;
using TinderForPets.Infrastructure;

namespace TinderForPets.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SwipeController : Controller
    {
        private readonly SwipeService _swipeService;
        private readonly IJwtProvider _jwtProvider;
        public SwipeController(SwipeService swipeService, IJwtProvider jwtProvider)
        {
            _swipeService = swipeService;
            _jwtProvider = jwtProvider;
        }

        [Authorize]
        [HttpPost("save")]
        public async Task<IResult> SaveSwipe([FromBody] SaveSwipeRequest request, CancellationToken cancellationToken) 
        {
            var validationTokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (validationTokenResult.IsFailure)
            {
                return validationTokenResult.ToProblemDetails();
            }

            var result = await _swipeService.SaveSwipeAsync(validationTokenResult.Value, request.PetSwipedOnProfileId, request.IsLike, cancellationToken);
            return result.IsSuccess ? Results.Created() : result.ToProblemDetails();
        }

    }
}
