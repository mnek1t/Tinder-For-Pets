using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinderForPets.API.Contracts.Profiles;
using TinderForPets.API.Extensions;
using TinderForPets.Application.DTOs;
using TinderForPets.Application.Services;
using TinderForPets.Infrastructure;

namespace TinderForPets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalProfileController : ControllerBase
    {
        private readonly AnimalProfileService _profileService;
        private readonly ImageHandlerService _imageResizerService;
        private readonly GeocodingService _geoCodingService;
        private readonly IJwtProvider _jwtProvider;

        public AnimalProfileController(AnimalProfileService profileService, ImageHandlerService imageResizerService, GeocodingService geoCodingService, IJwtProvider jwtProvider)
        {
            _profileService = profileService;
            _imageResizerService = imageResizerService;
            _geoCodingService = geoCodingService;
            _jwtProvider = jwtProvider;
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
        [HttpPost("animal/profile/create")]
        public async Task<IResult> CreateProfile([FromForm] CreateAnimalProfileRequest request) 
        {
            // Extract and Validate JWT Token
            var tokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (tokenResult.IsFailure) 
            {
                return tokenResult.ToProblemDetails();
            }

            // Get coordinates of pet / user location
            var geoCodingResult = await _geoCodingService.GetLocationCoordinates(request.City, request.Country);
            if (geoCodingResult.IsFailure) 
            {
                return geoCodingResult.ToProblemDetails();
            }

            // Create record in animal table
            var animalDto = new AnimalDto 
            { 
                OwnerId = tokenResult.Value, 
                AnimalTypeId = request.TypeId,
                BreedId = request.BreedId
            };

            var createAnimalResult = await _profileService.CreateAnimalAsync(animalDto);
            if (createAnimalResult.IsFailure) 
            {
                return createAnimalResult.ToProblemDetails();
            }

            // Create record in animal_profile table
            var animalProfileDto = new AnimalProfileDto 
            {
                AnimalId = createAnimalResult.Value,
                Name = request.Name,
                Description = request.Description,
                DateOfBirth = request.DateOfBirth,
                SexId = request.SexId,
                IsVaccinated = request.IsVaccinated,
                IsSterilized = request.IsSterilized,
                Country = request.Country,
                City = request.City,
                Latitude = geoCodingResult.Value.latitude,
                Longitude = geoCodingResult.Value.longitude,
                Height = request.Height,
                Weight = request.Weight
            };

            var createAnimalProfileResult = await _profileService.CreatePetProfile(animalProfileDto);
            if (createAnimalProfileResult.IsFailure) 
            {
                return createAnimalProfileResult.ToProblemDetails();
            }

            //Upload imagesto animal_images table
            var resizedImagesResult = await _imageResizerService.ResizeImages(request.Files, createAnimalProfileResult.Value);
            if (resizedImagesResult.IsFailure)
            {
                return resizedImagesResult.ToProblemDetails();
            }

            var uploadImageResult = await _imageResizerService.SaveImages(resizedImagesResult.Value);
            if (uploadImageResult.IsFailure)
            {
                return uploadImageResult.ToProblemDetails();
            }

            return Results.Ok(createAnimalProfileResult);
        }

        [Authorize]
        [HttpPost("animal/profile/update/{id}")]
        public async Task<IResult> UpdateProfile(Guid id, [FromBody] UpdateAnimalProfileRequest request)
        {
            var geoCodingResult = await _geoCodingService.GetLocationCoordinates(request.City, request.Country);
            if (geoCodingResult.IsFailure)
            {
                return geoCodingResult.ToProblemDetails();
            }
            var validationTokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (validationTokenResult.IsFailure) 
            {
                return validationTokenResult.ToProblemDetails();
            }

            var animalDto = new AnimalDto
            {
                Id = id,
                OwnerId = validationTokenResult.Value,
                AnimalTypeId = request.TypeId,
                BreedId = request.BreedId
            };

            var result = await _profileService.UpdateAnimal(animalDto);
            if (result.IsSuccess)
            {
                var animalProfileDto = new AnimalProfileDto
                {
                    AnimalId = id,
                    Name = request.Name,
                    Description = request.Description,
                    DateOfBirth = request.DateOfBirth,
                    SexId = request.SexId,
                    IsVaccinated = request.IsVaccinated,
                    IsSterilized = request.IsSterilized,
                    Country = request.Country,
                    City = request.City,
                    Latitude = geoCodingResult.Value.latitude,
                    Longitude = geoCodingResult.Value.longitude,
                    Height = request.Height,
                    Weight = request.Weight
                };

                result = await _profileService.UpdatePetProfile(animalProfileDto);
            }

            return result.IsSuccess ? Results.Ok(result) : result.ToProblemDetails();
        }

        [HttpPost("animal/image/upload")]
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
