using SharedKernel;
using TinderForPets.Application.DTOs;
using TinderForPets.Application.Interfaces;
using TinderForPets.Core;
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

        public async Task<Result<List<Sex>>> GetSexesAsync() 
        {
            var cacheKey = "GET_SEXES";
            var cachedData = await _cacheService.GetAsync<List<Sex>>(cacheKey);
            if (cachedData != null)
            {
                return Result.Success(cachedData);
            }

            var sexes = await _sexRepository.GetSexes();

            return Result.Success<List<Sex>>(sexes);
        }

        public async Task<Result<Guid>> CreatePetProfile(string name, string description, int age, string sex, bool isVaccinated, bool isSterilized, string breed, Guid userId) 
        {
            return null;
        }
    }
}
