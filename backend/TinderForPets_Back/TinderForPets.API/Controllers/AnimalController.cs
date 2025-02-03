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
    public class AnimalController : ControllerBase
    {
        private readonly AnimalService _animalService;
        private readonly ImageHandlerService _imageResizerService;
        private readonly GeocodingService _geoCodingService;
        private readonly S2GeometryService _s2GeometryService;
        private readonly IJwtProvider _jwtProvider;

        public AnimalController(
            AnimalService profileService,
            ImageHandlerService imageResizerService,
            GeocodingService geoCodingService,
            IJwtProvider jwtProvider,
            S2GeometryService s2GeometryService)
        {
            _animalService = profileService;
            _imageResizerService = imageResizerService;
            _geoCodingService = geoCodingService;
            _jwtProvider = jwtProvider;
            _s2GeometryService = s2GeometryService;
        }

        [Authorize]
        [HttpGet("animal/data")]
        public async Task<IResult> GetProfileData(CancellationToken cancellationToken) 
        {
            
            var validationTokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (validationTokenResult.IsFailure)
            {
                return validationTokenResult.ToProblemDetails();
            }

            var animalProfileDetailsResult = await _animalService.GetAnimalProfileDetails(validationTokenResult.Value, cancellationToken);

            if (animalProfileDetailsResult.IsFailure)
            {
                return animalProfileDetailsResult.ToProblemDetails();
            }

            return Results.Ok(animalProfileDetailsResult.Value);
        }
        [Authorize]
        [HttpGet("animal/image")]
        public async Task<IResult> GetProfileImage(CancellationToken cancellationToken)
        {
            var validationTokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (validationTokenResult.IsFailure)
            {
                return validationTokenResult.ToProblemDetails();
            }

            var animalProfileResult = await _animalService.GetAnimalProfileId(validationTokenResult.Value, cancellationToken);

            if (animalProfileResult.IsFailure)
            {
                return animalProfileResult.ToProblemDetails();
            }

            var result = await _animalService.GetAnimalImageAsync(animalProfileResult.Value.Id, cancellationToken);
            var animalImageDto = result.Value;
            return Results.Ok(File(animalImageDto.ImageData, animalImageDto.ImageFormat));

        }

        [HttpGet("animal-types")]
        public async Task<IResult> GetAnimalTypes(CancellationToken cancellationToken)
        {
            var result = await _animalService.GetAnimalTypesAsync(cancellationToken);
            var animalTypes = result.Value;
            return Results.Ok(animalTypes);
        }

        [HttpGet("breeds/{id}")]
        public async Task<IResult> GetBreedsByAnimalId(int id, CancellationToken cancellationToken)
        {
            var result = await _animalService.GetAnimalBreedByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpGet("sexes")]
        public async Task<IResult> GetSexes(CancellationToken cancellationToken)
        {
            var result = await _animalService.GetSexesAsync(cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        [Authorize]
        [HttpPost("animal/profile/create")]
        public async Task<IResult> CreateProfile([FromForm] CreateAnimalProfileRequest request, CancellationToken cancellationToken) 
        {
            // Extract and Validate JWT Token
            var tokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (tokenResult.IsFailure) 
            {
                return tokenResult.ToProblemDetails();
            }

            // Get coordinates of pet / user location
            var geoCodingResult = await _geoCodingService.GetLocationCoordinates(request.City, request.Country, cancellationToken);
            if (geoCodingResult.IsFailure) 
            {
                return geoCodingResult.ToProblemDetails();
            }

            // Calculate S2 Cell - prepare for S2Gometryibrary algorithm
            var s2cellId = _s2GeometryService.GetS2CellId(geoCodingResult.Value.latitude, geoCodingResult.Value.longitude); 
            
            // Create record in animal table
            var animalDto = new AnimalDto 
            { 
                OwnerId = tokenResult.Value, 
                AnimalTypeId = request.AnimalTypeId,
                BreedId = request.BreedId
            };

            var createAnimalResult = await _animalService.CreateAnimalAsync(animalDto, cancellationToken);
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
                S2CellId = s2cellId,
                Height = request.Height,
                Weight = request.Weight
            };

            var createAnimalProfileResult = await _animalService.CreatePetProfile(animalProfileDto, cancellationToken);
            if (createAnimalProfileResult.IsFailure) 
            {
                return createAnimalProfileResult.ToProblemDetails();
            }

            //Upload image dto animal_images table
            var resizedImagesResult = await _imageResizerService.ResizeImages(request.File, createAnimalProfileResult.Value, cancellationToken);
            if (resizedImagesResult.IsFailure)
            {
                return resizedImagesResult.ToProblemDetails();
            }

            var uploadImageResult = await _imageResizerService.SaveImages(resizedImagesResult.Value, cancellationToken);
            if (uploadImageResult.IsFailure)
            {
                return uploadImageResult.ToProblemDetails();
            }

            return Results.Ok(createAnimalProfileResult);
        }

        [Authorize]
        [HttpPost("animal/profile/update/{id}")]
        public async Task<IResult> UpdateProfile(Guid id, [FromBody] UpdateAnimalProfileRequest request, CancellationToken cancellationToken)
        {
            var geoCodingResult = await _geoCodingService.GetLocationCoordinates(request.City, request.Country, cancellationToken);
            if (geoCodingResult.IsFailure)
            {
                return geoCodingResult.ToProblemDetails();
            }

            // Calculate S2 Cell - prepare for S2Gometryibrary algorithm
            var s2cellId = _s2GeometryService.GetS2CellId(geoCodingResult.Value.latitude, geoCodingResult.Value.longitude);

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

            var result = await _animalService.UpdateAnimal(animalDto, cancellationToken);
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
                    S2CellId = s2cellId,
                    //Height = request.Height,
                    //Weight = request.Weight
                };

                result = await _animalService.UpdatePetProfile(animalProfileDto, cancellationToken);
            }

            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        }

        [HttpPost("animal/image/upload")]
        public async Task<IResult> UploadAnimalMedia([FromForm] AnimalMediaUploadRequest request, CancellationToken cancellationToken) 
        {
            var tokenResult = _jwtProvider.ValidateAuthTokenAndExtractUserId(HttpContext);
            if (tokenResult.IsFailure)
            {
                return tokenResult.ToProblemDetails();
            }
            var animalProfileIdResult = await _animalService.GetAnimalProfileId(tokenResult.Value, cancellationToken);
            var resizedImagesResult = await _imageResizerService.ResizeImages(request.File, animalProfileIdResult.Value.Id, cancellationToken, request.Description);
            if (resizedImagesResult.IsFailure) 
            {
                return resizedImagesResult.ToProblemDetails();
            }

            var uploadImageResult = await _imageResizerService.SaveImages(resizedImagesResult.Value, cancellationToken);
            return uploadImageResult.IsSuccess ? Results.NoContent() : uploadImageResult.ToProblemDetails();
        }
    }
}
