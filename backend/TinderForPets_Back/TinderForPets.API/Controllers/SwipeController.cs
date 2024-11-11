using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinderForPets.API.Contracts.Swipes;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;

namespace TinderForPets.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SwipeController : Controller
    {
        private readonly SwipeService _swipeService;
        public SwipeController(SwipeService swipeService)
        {
            _swipeService = swipeService;
        }

        [Authorize]
        [HttpPost("save")]
        public async Task<IResult> SaveSwipe([FromBody] SaveSwipeRequest request, CancellationToken cancellationToken) 
        {
            var result = await _swipeService.SaveSwipeAsync(request.PetSwiperProfileId, request.PetSwipedOnProfileId, request.IsLike, cancellationToken);
            return result.IsSuccess ? Results.Created() : result.ToProblemDetails();
        }

    }
}
