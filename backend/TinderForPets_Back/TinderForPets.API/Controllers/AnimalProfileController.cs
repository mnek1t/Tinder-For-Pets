using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinderForPets.API.Contracts.Profiles;
using TinderForPets.API.Extensions;
using TinderForPets.Application.Services;
using TinderForPets.Data.Entities;

namespace TinderForPets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalProfileController : ControllerBase
    {
        private readonly AnimalProfileService _profileService;
        private readonly ImageHandlerService _imageResizerService;

        public AnimalProfileController(AnimalProfileService profileService, ImageHandlerService imageResizerService)
        {
            _profileService = profileService;
            _imageResizerService = imageResizerService;
        }

        [HttpGet("animal-types")]
        public async Task<IResult> GetAnimalTypes()
        {
            var result = await _profileService.GetAnimalTypesAsync();
            var animalTypes = result.Value;
            return Results.Ok(animalTypes);
        }

        [HttpGet("breeds/{id}")]
        public async Task<IResult> GetBreedsByAnimalId(int id)
        {
            var result = await _profileService.GetAnimalBreedByIdAsync(id);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpGet("sexes")]
        public async Task<IResult> GetSexes()
        {
            var result = await _profileService.GetSexesAsync();
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        [Authorize]
        [HttpPost("animal/create-profile")]
        public async Task<IResult> CreateProfile([FromBody] CreateAnimalProfileRequest request) 
        {
            var result = await _profileService.CreateAnimalAsync(request.OwnerId, request.TypeId, request.BreedId);
            if (result.IsSuccess) 
            {
                result = await _profileService.CreatePetProfile(result.Value, request.Name, request.Description, request.Age, request.SexId, request.IsVaccinated, request.IsSterilized);

            }
            return result.IsSuccess ? Results.Ok(result) : result.ToProblemDetails();
        }

        [HttpPost("animal/image-upload")]
        public async Task<IResult> UploadAnimalMedia([FromForm] AnimalMediaUploadRequest request) 
        {
            var resizedImagesResult = await _imageResizerService.ResizeImages(request.Files, request.AnimalProfileId, request.Description);
            if (resizedImagesResult.IsFailure) 
            {
                return resizedImagesResult.ToProblemDetails();
            }

            var uploadImageResult = await _imageResizerService.SaveImages(resizedImagesResult.Value);
            return uploadImageResult.IsSuccess ? Results.Ok(uploadImageResult) : uploadImageResult.ToProblemDetails();
        }
    }
}
