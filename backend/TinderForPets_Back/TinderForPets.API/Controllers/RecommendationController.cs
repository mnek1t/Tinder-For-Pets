using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;
using TinderForPets.Infrastructure;

namespace TinderForPets.API.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly RecommendationService _recommendationService;
        private readonly IJwtProvider _jwtProvider;

        public RecommendationController(
            RecommendationService recommendationService,
            IJwtProvider jwtProvider)
        {
            _recommendationService = recommendationService;
            _jwtProvider = jwtProvider;
        }
        //[Authorize]
        //[HttpGet("matches")]
        //public async Task<IResult> GetMatches() { }


        [Authorize]
        [HttpGet("recommendations")]
        // TODO: Do not pass radiusKm as a query parameter! RadiusKm is a user preference so, it is neccessary to create additional column
        public async Task<IResult> GetRecommendations([FromQuery] double radiusKm, CancellationToken cancellationToken)
        {
            var tokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (tokenResult.IsFailure)
            {
                return tokenResult.ToProblemDetails();
            }
            var recommendationsResult = await _recommendationService.GetRecommendationsForUserAsync(tokenResult.Value, radiusKm, cancellationToken);
            if (recommendationsResult.IsFailure) 
            {
                return recommendationsResult.ToProblemDetails();
            }
            return Results.Ok(recommendationsResult.Value);
        }
    }
}
