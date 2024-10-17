using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinderForPets.API.Contracts.Profiles;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;

namespace TinderForPets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalProfileController : ControllerBase
    {
        private readonly AnimalProfileService _profileService;

        public AnimalProfileController(AnimalProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IResult> GetAnimalTypes()
        {
            var result = await _profileService.GetAnimalTypesAsync();
            var animalTypes = result.Value;
            return Results.Ok(animalTypes);
        }

        [HttpGet("api/[controller]/breeds/{id}")]
        public async Task<IResult> GetBreedsByAnimalId(int id)
        {
            var result = await _profileService.GetAnimalBreedByIdAsync(id);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpGet("api/[controller]/sex")]
        public async Task<IResult> GetSexes()
        {
            var result = await _profileService.GetSexesAsync();
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        [Authorize]
        [HttpPost("create-profile")]
        public async Task<IResult> CreateProfile([FromBody] CreateAnimalProfileRequest request) 
        {
            await _profileService.CreatePetProfile(request.Name, request.Description, request.Age, request.Sex, request.IsVaccinated, request.IsSterilized, request.Breed, request.OwnerId);
            return null;
        }
    }
}
