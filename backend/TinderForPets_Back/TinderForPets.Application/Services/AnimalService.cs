﻿using AutoMapper;
using SharedKernel;
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
                return Result.Failure<List<AnimalTypeDto>>(new Error("400", "Operation canceled"));
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
            catch (OperationCanceledException)
            {
                return Result.Failure<List<BreedDto>>(new Error("400", "Operation canceled"));
            }
        }

        public async Task<Result<List<SexDto>>> GetSexesAsync(CancellationToken cancellationToken) 
        {
            try
            {
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
                return Result.Failure<List<SexDto>>(new Error("400", "Operation canceled"));
            }
        }

        public async Task<Result<AnimalImageDto>> GetAnimalImageAsync(Guid animalProfileId, CancellationToken cancellationToken)
        {
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

        public async Task<Result<Guid>> GetAnimalProfileId(Guid ownerId,CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var animalProfileId = await _animalProfileRepository.GetAnimalProfileByOwnerIdAsync(ownerId, cancellationToken);
                return Result.Success<Guid>(animalProfileId);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<Guid>(new Error("400", "Operation canceled"));
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(new Error("400", ex.Message));
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
                return Result.Failure<Guid>(new Error("400", "Operation canceled"));
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
                animalProfileDto.Height,
                animalProfileDto.Weight);

                var animalProifleEntity = _mapper.Map<AnimalProfile>(animalProfileModel);
                var resultId = await _animalProfileRepository.CreateAsync(animalProifleEntity, cancellationToken);
                return Guid.Empty == resultId ? Result.Failure<Guid>(AnimalProfileErrors.NotCreatedProfile) : Result.Success<Guid>(resultId);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<Guid>(new Error("400", "Operation canceled"));
            }
        }
        #endregion

        #region Update Actions
        public async Task<Result<string>> UpdateAnimal(AnimalDto animalDto, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var animalModel = AnimalModel.Create(animalDto.Id, animalDto.OwnerId, animalDto.AnimalTypeId, animalDto.BreedId);
                var animalEntity = _mapper.Map<Animal>(animalModel);
                await _animalRepository.UpdateAsync(animalEntity, cancellationToken);
                
                return Result.Success<string>($"Success. Pet with {animalDto.Id} id was successfully updated");
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<string>(new Error("400", "Operation canceled"));
            }
            catch (AnimalNotFoundException ex)
            {
                return Result.Failure<string>(AnimalProfileErrors.NotUpdated(ex.Message));
            }
        }

        public async Task<Result<string>> UpdatePetProfile(AnimalProfileDto animalProfileDto, CancellationToken cancellationToken)
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
                animalProfileDto.Height,
                animalProfileDto.Weight);
                var animalProifleEntity = _mapper.Map<AnimalProfile>(animalProfileModel);
                await _animalProfileRepository.UpdateAsync(animalProifleEntity, cancellationToken);
                return Result.Success<string>($"Success. Pet with {animalProfileDto.AnimalId} id was successfully updated");
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<string>(new Error("400", "Operation canceled"));
            }
            catch (AnimalNotFoundException ex)
            {
                return Result.Failure<string>(AnimalProfileErrors.NotUpdated(ex.Message));
            }
        }
        #endregion
    }
}