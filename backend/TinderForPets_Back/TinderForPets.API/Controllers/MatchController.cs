using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;
using TinderForPets.Infrastructure;

namespace TinderForPets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchService;
        private readonly IJwtProvider _jwtProvider;
        public MatchController(MatchService matchService, IJwtProvider jwtProvider)
        {
            _matchService = matchService;
            _jwtProvider = jwtProvider;
        }
        [Authorize]
        [HttpGet]
        public async Task<IResult> GetMatches(CancellationToken cancellationToken)
        {
            var validationTokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (validationTokenResult.IsFailure)
            {
                return validationTokenResult.ToProblemDetails();
            }
            var animalProfilesResult = await _matchService.GetMatchesProfilesData(validationTokenResult.Value, cancellationToken);
            return Results.Ok(animalProfilesResult.Value);
        }
    }
}
