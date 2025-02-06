using AutoMapper;
using SharedKernel;
using System.Collections.Generic;
using TinderForPets.Application.DTOs;
using TinderForPets.Application.Interfaces;
using TinderForPets.Core;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Application.Services
{
    public class AnimalService
    {
        private readonly IAnimalProfileRepository _animalProfileRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IAnimalTypeRepository _animalTypeRepository;
        private readonly IBreedRepository _breedRepository;
        private readonly ICacheService _cacheService;
        private readonly ISexRepository _sexRepository;
        private readonly IAnimalImageRepository _animalImageRepository;
        private readonly IMapper _mapper;

        public AnimalService(
            IAnimalProfileRepository animalProfileRepository,
            IAnimalRepository animalRepository,
            ICacheService cacheService, 
            IAnimalTypeRepository animalTypeRepository, 
            IBreedRepository breedRepository,
            ISexRepository sexRepository,
            IAnimalImageRepository animalImageRepository,
            IMapper mapper)
        {
            _animalProfileRepository = animalProfileRepository;
            _animalRepository = animalRepository;
            _animalTypeRepository = animalTypeRepository;
            _cacheService = cacheService;
            _breedRepository = breedRepository;
            _sexRepository = sexRepository;
            _animalImageRepository = animalImageRepository;
            _mapper = mapper;
        }
        #region Get Data Service
        public async Task<Result<List<AnimalTypeDto>>> GetAnimalTypesAsync(CancellationToken cancellationToken) 
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var cacheKey = "GET_ANIMAL_TYPES";
                var cachedData = await _cacheService.GetAsync<List<AnimalTypeDto>>(cacheKey);
                if (cachedData != null)
                {
                    return Result.Success(cachedData);
                }

                var animalTypes = await _animalTypeRepository.GetAllAnimalTypesAsync(cancellationToken);
                var animalTypeDto = animalTypes.Select(a => new AnimalTypeDto
                {
                    Id = a.Id,
                    TypeName = a.TypeName

                }).ToList();

                await _cacheService.SetAsync<List<AnimalTypeDto>>(cacheKey, animalTypeDto, TimeSpan.FromMinutes(5));

                return Result.Success<List<AnimalTypeDto>>(animalTypeDto);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<List<AnimalTypeDto>>(OperationCancellationErrors.OperationCancelled);
            }

        }

        public async Task<Result<List<BreedDto>>> GetAnimalBreedByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var cacheKey = "GET_ANIMAL_BREEDS_BY_TYPE_ID_" + id.ToString();
                var cachedData = await _cacheService.GetAsync<List<BreedDto>>(cacheKey);
                if (cachedData != null)
                {
                    return Result.Success(cachedData);
                }

                var result = await _breedRepository.GetBreedsByTypeIdAsync(id, cancellationToken);

                var breedDto = result.Select(b => new BreedDto
                {
                    Id = b.Id,
                    BreedName = b.BreedName

                }).ToList();

                await _cacheService.SetAsync<List<BreedDto>>(cacheKey, breedDto, TimeSpan.FromMinutes(5));

                return Result.Success<List<BreedDto>>(breedDto);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<List<BreedDto>>(OperationCancellationErrors.OperationCancelled);
            }
            catch (BreedNotFoundException) 
            {
                return Result.Failure<List<BreedDto>>(AnimalProfileErrors.BreedNotFound(id));
            }
        }

        public async Task<Result<List<SexDto>>> GetSexesAsync(CancellationToken cancellationToken) 
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var cacheKey = "GET_SEXES";
                var cachedData = await _cacheService.GetAsync<List<SexDto>>(cacheKey);
                if (cachedData != null)
                {
                    return Result.Success(cachedData);
                }

                var sexesData = await _sexRepository.GetSexes(cancellationToken);
                var sexesDtos = sexesData.Select(s => new SexDto
                {
                    Id = s.Id,
                    SexName = s.SexName
                }).ToList();

                await _cacheService.SetAsync<List<SexDto>>(cacheKey, sexesDtos, TimeSpan.FromMinutes(5));

                return Result.Success<List<SexDto>>(sexesDtos);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<List<SexDto>>(OperationCancellationErrors.OperationCancelled);
            }
        }

        public async Task<Result<AnimalImageDto>> GetAnimalImageAsync(Guid animalProfileId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var cacheKey = $"GET_ANIMAL_IMAGE {animalProfileId}";
                var cachedData = await _cacheService.GetAsync<AnimalImageDto>(cacheKey);
                if (cachedData != null)
                {
                    return Result.Success(cachedData);
                }

                var animalImageEntity = await _animalImageRepository.GetAnimalImageAsync(animalProfileId, cancellationToken);
                var animalImageDto = new AnimalImageDto()
                {
                    ImageData = animalImageEntity.ImageData,
                    ImageFormat = animalImageEntity.ImageFormat
                };

                await _cacheService.SetAsync<AnimalImageDto>(cacheKey, animalImageDto, TimeSpan.FromMinutes(5));

                return Result.Success<AnimalImageDto>(animalImageDto);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<AnimalImageDto>(OperationCancellationErrors.OperationCancelled);
            }
            catch (AnimalNotFoundException)
            {
                return Result.Failure<AnimalImageDto>(AnimalProfileErrors.ImageIsNotFound);
            }
        }
        public async Task<Result<AnimalDetailsDto>> GetAnimalProfileDetails(Guid ownerId, CancellationToken cancellationToken) 
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var cacheKey = "GET_ANIMAL_PROFILE_DETAILS_BY_OWNER" + ownerId.ToString();
                var cachedProfile = await _cacheService.GetAsync<AnimalDetailsDto>(cacheKey);
                if (cachedProfile is not null)
                {
                    return Result.Success<AnimalDetailsDto>(cachedProfile);
                }

                var animalProfileDetails = await _animalProfileRepository.GetAnimalProfileDetails(ownerId, cancellationToken);

                var animalDetailsDto = new AnimalDetailsDto()
                {
                    Animal = new AnimalDto()
                    {
                        Id = animalProfileDetails.Animal.Id,
                        Breed = animalProfileDetails.Animal.Breed.BreedName,
                        AnimalType = animalProfileDetails.Animal.Type.TypeName
                    },
                    Profile = new AnimalProfileDto
                    {
                        Id = animalProfileDetails.Id,
                        Name = animalProfileDetails.Name,
                        Description = animalProfileDetails.Description,
                        Age = animalProfileDetails.Age,
                        DateOfBirth = animalProfileDetails.DateOfBirth,
                        Sex = animalProfileDetails.Sex.SexName,
                        IsSterilized = animalProfileDetails.IsSterilized,
                        IsVaccinated = animalProfileDetails.IsVaccinated,
                        City = animalProfileDetails.City,
                        Country = animalProfileDetails.Country,
                    },
                    Images = animalProfileDetails.Images.Select(i => new AnimalImageDto
                    {
                        ImageData = i.ImageData,
                        ImageFormat = i.ImageFormat
                    }).ToList(),
                };
                await _cacheService.SetAsync<AnimalDetailsDto>(cacheKey, animalDetailsDto, TimeSpan.FromHours(1));
                return Result.Success<AnimalDetailsDto>(animalDetailsDto);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<AnimalDetailsDto>(OperationCancellationErrors.OperationCancelled);
            }
            catch (AnimalNotFoundException) 
            {
                return Result.Failure<AnimalDetailsDto>(AnimalProfileErrors.NotFound);
            }
        }

        public async Task<Result<AnimalProfileModel>> GetAnimalProfileId(Guid ownerId,CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var animalProfile = await _animalProfileRepository.GetAnimalProfileByOwnerIdAsync(ownerId, cancellationToken);
                var animalProfileModel = _mapper.Map<AnimalProfileModel>(animalProfile);
                return Result.Success<AnimalProfileModel>(animalProfileModel);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<AnimalProfileModel>(OperationCancellationErrors.OperationCancelled);
            }
        }   
        #endregion

        #region Create Actions
        public async Task<Result<Guid>> CreateAnimalAsync(AnimalDto animalDto, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var animalModel = AnimalModel.Create(Guid.NewGuid(), animalDto.OwnerId, animalDto.AnimalTypeId, animalDto.BreedId);
                var animalEntity = _mapper.Map<Animal>(animalModel);
                var resultId = await _animalRepository.CreateAsync(animalEntity, cancellationToken);
                return Guid.Empty == resultId ? Result.Failure<Guid>(AnimalProfileErrors.NotCreatedAnimal) : Result.Success<Guid>(resultId);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<Guid>(OperationCancellationErrors.OperationCancelled);
            }
        }

        public async Task<Result<Guid>> CreatePetProfile(AnimalProfileDto animalProfileDto, CancellationToken cancellationToken) 
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var animalProfileModel = AnimalProfileModel.Create(
                Guid.NewGuid(),
                animalProfileDto.AnimalId,
                animalProfileDto.Name,
                animalProfileDto.Description,
                animalProfileDto.DateOfBirth,
                animalProfileDto.SexId,
                animalProfileDto.IsVaccinated,
                animalProfileDto.IsSterilized,
                animalProfileDto.Country,
                animalProfileDto.City,
                animalProfileDto.Latitude,
                animalProfileDto.Longitude,
                animalProfileDto.S2CellId,
                animalProfileDto.Height,
                animalProfileDto.Weight);

                var animalProifleEntity = _mapper.Map<AnimalProfile>(animalProfileModel);
                var resultId = await _animalProfileRepository.CreateAsync(animalProifleEntity, cancellationToken);
                return Guid.Empty == resultId ? Result.Failure<Guid>(AnimalProfileErrors.NotCreatedProfile) : Result.Success<Guid>(resultId);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<Guid>(OperationCancellationErrors.OperationCancelled);
            }
        }
        #endregion

        #region Update Actions
        public async Task<Result<AnimalDto>> UpdateAnimal(AnimalDto animalDto, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var animalModel = AnimalModel.Create(animalDto.Id, animalDto.OwnerId, animalDto.AnimalTypeId, animalDto.BreedId);
                var animalEntity = _mapper.Map<Animal>(animalModel);
                var updatedAnimal = await _animalRepository.UpdateAsync(animalEntity, cancellationToken);
                var updateAnimalDto = new AnimalDto
                {
                    Id = updatedAnimal.Id,
                    Breed = updatedAnimal.Breed.BreedName,
                    AnimalType = updatedAnimal.Type.TypeName,
                    OwnerId = updatedAnimal.UserId
                };
                return Result.Success<AnimalDto>(updateAnimalDto);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<AnimalDto>(OperationCancellationErrors.OperationCancelled);
            }
            catch (AnimalNotFoundException)
            {
                return Result.Failure<AnimalDto>(AnimalProfileErrors.NotUpdated);
            }
        }

        public async Task<Result<AnimalProfileDto>> UpdatePetProfile(AnimalProfileDto animalProfileDto, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var animalProfileModel = AnimalProfileModel.Create(
                animalProfileDto.Id,
                animalProfileDto.AnimalId,
                animalProfileDto.Name,
                animalProfileDto.Description,
                animalProfileDto.DateOfBirth,
                animalProfileDto.SexId,
                animalProfileDto.IsVaccinated,
                animalProfileDto.IsSterilized,
                animalProfileDto.Country,
                animalProfileDto.City,
                animalProfileDto.Latitude,
                animalProfileDto.Longitude,
                animalProfileDto.S2CellId,
                animalProfileDto.Height,
                animalProfileDto.Weight);
                var animalProifleEntity = _mapper.Map<AnimalProfile>(animalProfileModel);
                var updateAnimalProfile = await _animalProfileRepository.UpdateAsync(animalProifleEntity, cancellationToken);
                var updateAnimalProfileDto = new AnimalProfileDto 
                {
                    Id = updateAnimalProfile.Id,
                    Name = updateAnimalProfile.Name,
                    Description = updateAnimalProfile.Description,
                    Age = updateAnimalProfile.Age,
                    DateOfBirth = updateAnimalProfile.DateOfBirth,
                    Sex = updateAnimalProfile.Sex.SexName,
                    IsSterilized = updateAnimalProfile.IsSterilized,
                    IsVaccinated = updateAnimalProfile.IsVaccinated,
                    City = updateAnimalProfile.City,
                    Country = updateAnimalProfile.Country,
                };
                return Result.Success<AnimalProfileDto>(updateAnimalProfileDto);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<AnimalProfileDto>(OperationCancellationErrors.OperationCancelled);
            }
            catch (AnimalNotFoundException)
            {
                return Result.Failure<AnimalProfileDto>(AnimalProfileErrors.NotUpdated);
            }
        }
        public async Task UpdateProfileDetailsCache(AnimalDto animalDto, AnimalProfileDto animalProfileDto, CancellationToken cancellationToken) 
        {
            var cacheKey = "GET_ANIMAL_PROFILE_DETAILS_BY_OWNER" + animalDto.OwnerId.ToString();
            var cachedData = await _cacheService.GetAsync<AnimalDetailsDto>(cacheKey);
            if (cachedData is null) 
            {
                return;
            }
            var freshAnimalDetailsDto = new AnimalDetailsDto()
            {
                Animal = animalDto,
                Profile = animalProfileDto,
                Images = cachedData.Images.Select(i => new AnimalImageDto
                {
                    ImageData = i.ImageData,
                    ImageFormat = i.ImageFormat
                }).ToList(),
            };
            await _cacheService.SetAsync<AnimalDetailsDto>(cacheKey, freshAnimalDetailsDto, TimeSpan.FromHours(1));
        }

        public async Task UpdateProfileImagesCache(AnimalImageDto animalimageDto, Guid ownerId, CancellationToken cancellationToken)
        {
            var cacheKey = "GET_ANIMAL_PROFILE_DETAILS_BY_OWNER" + ownerId.ToString();
            var cachedData = await _cacheService.GetAsync<AnimalDetailsDto>(cacheKey);
            var freshAnimalDetailsDto = new AnimalDetailsDto()
            {
                Animal = cachedData.Animal,
                Profile = cachedData.Profile,
                Images = new List<AnimalImageDto>
                {
                    new AnimalImageDto{
                        ImageData = animalimageDto.ImageData,
                        ImageFormat = animalimageDto.ImageFormat
                    }
                }
            };
            await _cacheService.SetAsync<AnimalDetailsDto>(cacheKey, freshAnimalDetailsDto, TimeSpan.FromHours(1));
        }
        #endregion

        //private static int CalculateAge(DateOnly birthDate) 
        //{
        //    var today = DateOnly.FromDateTime(DateTime.Today);
        //    var age = today.Year - birthDate.Year;
        //    if (birthDate > today.AddYears(-age)) age--;
        //    return age;
        //}
    }
}
