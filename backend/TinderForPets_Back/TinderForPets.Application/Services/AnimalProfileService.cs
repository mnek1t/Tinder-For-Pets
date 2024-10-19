using SharedKernel;
using TinderForPets.Application.DTOs;
using TinderForPets.Application.Interfaces;
using TinderForPets.Core;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;
using TinderForPets.Data.Repositories;

namespace TinderForPets.Application.Services
{
    public class AnimalProfileService
    {
        private readonly IAnimalProfileRepository _animalProfileRepository;
        private readonly IAnimalTypeRepository _animalTypeRepository;
        private readonly IBreedRepository _breedRepository;
        private readonly ICacheService _cacheService;
        private readonly ISexRepository _sexRepository;
        public AnimalProfileService(
            IAnimalProfileRepository animalProfileRepository, 
            ICacheService cacheService, 
            IAnimalTypeRepository animalTypeRepository, 
            IBreedRepository breedRepository,
            ISexRepository sexRepository)
        {
            _animalProfileRepository = animalProfileRepository;
            _animalTypeRepository = animalTypeRepository;
            _cacheService = cacheService;
            _breedRepository = breedRepository;
            _sexRepository = sexRepository;
        }

        public async Task<Result<List<AnimalTypeDto>>> GetAnimalTypesAsync() 
        {
            var cacheKey = "GET_ANIMAL_TYPES";
            var cachedData = await _cacheService.GetAsync<List<AnimalTypeDto>>(cacheKey);
            if (cachedData != null) 
            {
                return Result.Success(cachedData);
            }
            var result = await _animalTypeRepository.GetAllAnimalTypesAsync();
            var animalTypeDto = result.Value.Select(a => new AnimalTypeDto
            {
                Id = a.Id,
                TypeName = a.TypeName

            }).ToList();

            await _cacheService.SetAsync<List<AnimalTypeDto>>(cacheKey, animalTypeDto, TimeSpan.FromMinutes(5));

            return Result.Success<List<AnimalTypeDto>>(animalTypeDto);
        }

        public async Task<Result<List<BreedDto>>> GetAnimalBreedByIdAsync(int id)
        {
            var cacheKey = "GET_ANIMAL_BREEDS_BY_TYPE_ID_" + id.ToString();
            var cachedData = await _cacheService.GetAsync<List<BreedDto>>(cacheKey);
            if (cachedData != null)
            {
                return Result.Success(cachedData);
            }

            var result = await _breedRepository.GetBreedsByTypeIdAsync(id);
            if (result == null) 
            {
                return Result.Failure<List<BreedDto>>(AnimalProfileErrors.BreedNotFound(id));
            }

            var breedDto = result.Select(b => new BreedDto
            {
                Id = b.Id,
                BreedName = b.BreedName

            }).ToList();
            
            await _cacheService.SetAsync<List<BreedDto>>(cacheKey, breedDto, TimeSpan.FromMinutes(5));

            return Result.Success<List<BreedDto>>(breedDto);
        }

        public async Task<Result<List<SexDto>>> GetSexesAsync() 
        {
            var cacheKey = "GET_SEXES";
            var cachedData = await _cacheService.GetAsync<List<SexDto>>(cacheKey);
            if (cachedData != null)
            {
                return Result.Success(cachedData);
            }

            var sexesData = await _sexRepository.GetSexes();
            var sexesDtos = sexesData.Select(s => new SexDto
            {
                Id = s.Id,
                SexName = s.SexName
            }).ToList();

            await _cacheService.SetAsync<List<SexDto>>(cacheKey, sexesDtos, TimeSpan.FromMinutes(5));

            return Result.Success<List<SexDto>>(sexesDtos);
        }

        public async Task<Result<Guid>> CreateAnimalAsync(AnimalDto animalDto) 
        {
            var animalModel = AnimalModel.Create(Guid.NewGuid(), animalDto.OwnerId, animalDto.AnimalTypeId, animalDto.BreedId);
            var resultId = await _animalProfileRepository.CreateAnimalAsync(animalModel);
            return Guid.Empty == resultId ? Result.Failure<Guid>(AnimalProfileErrors.NotCreatedAnimal) : Result.Success<Guid>(resultId);
        }

        public async Task<Result<Guid>> CreatePetProfile(AnimalProfileDto animalProfileDto) 
        {
            var animalProfileModel = AnimalProfileModel.Create(
                Guid.NewGuid(), 
                animalProfileDto.AnimalId, 
                animalProfileDto.Name, 
                animalProfileDto.Description,
                animalProfileDto.Age, 
                animalProfileDto.SexId,
                animalProfileDto.IsVaccinated, 
                animalProfileDto.IsSterilized, 
                animalProfileDto.Country, 
                animalProfileDto.City,
                animalProfileDto.Latitude,
                animalProfileDto.Longitude,
                animalProfileDto.Height,
                animalProfileDto.Width);

            var resultId = await _animalProfileRepository.CreateProfileAsync(animalProfileModel);
            return Guid.Empty == resultId ? Result.Failure<Guid>(AnimalProfileErrors.NotCreatedProfile) : Result.Success<Guid>(resultId);
        }
    }
}
