using AutoMapper;
using SharedKernel;
using System.Collections.Immutable;
using System.Threading;
using TinderForPets.Application.DTOs;
using TinderForPets.Application.Interfaces;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;
using TinderForPets.Data.Repositories;

namespace TinderForPets.Application.Services
{
    public class RecommendationService
    {
        private readonly IMapper _mapper;
        private readonly IAnimalProfileRepository _profileRepository;
        private readonly ICacheService _cacheService;
        private readonly SwipeService _swipeService;
        private readonly S2GeometryService _s2GeometryService;
        public RecommendationService(
            IAnimalProfileRepository animalProfileRepository,
            ICacheService cacheService,
            S2GeometryService s2GeometryService,
            SwipeService swipeService,
            IMapper mapper)
        {
            _profileRepository = animalProfileRepository;
            _cacheService = cacheService;
            _s2GeometryService = s2GeometryService;
            _swipeService = swipeService;
            _mapper = mapper;
        }
        private async Task<AnimalProfile> GetAnimalProfile(Guid userId, CancellationToken cancellationToken) 
        {
            cancellationToken.ThrowIfCancellationRequested();
            var cacheKey = $"animal_details_of_owner{userId}";
            var animalProfile = await _cacheService.GetAsync<AnimalProfile>(cacheKey);
            if (animalProfile == null)
            {
                animalProfile = await _profileRepository.GetAnimalProfileByOwnerIdAsync(userId, cancellationToken);
                // TODO: If I save all details, they can be updated, so I need to update the cache then 
                await _cacheService.SetAsync<AnimalProfile> (cacheKey, animalProfile, TimeSpan.FromHours(24));
            }

            return animalProfile;
        }
        public async Task<Result<ImmutableList<AnimalDetailsDto>>> GetRecommendationsForUserAsync(Guid userId, double radiusKm, CancellationToken cancellationToken) 
        {
            // TODO: Cache data
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var animalProfile = await GetAnimalProfile(userId, cancellationToken);
                var nearbyCells = _s2GeometryService.GetNearbyCellIds(animalProfile.Latitude, animalProfile.Longitude, radiusKm);
                var swipedProfileIdsResult = await _swipeService.GetSwipedProfiles(animalProfile.Id);
                if (swipedProfileIdsResult.IsFailure) 
                {
                    return Result.Failure<ImmutableList<AnimalDetailsDto>>(swipedProfileIdsResult.Error);
                }

                var swipedProfileIds = swipedProfileIdsResult.Value;
                
                var filter = new AnimalRecommendationFilter
                {
                    OppositeSexId = animalProfile.SexId == (int)Gender.Male ? (int)Gender.Female: (int)Gender.Male,
                    NearbyS2CellIds = nearbyCells,
                    SwipedProfileIds = swipedProfileIds,
                    BreedId = animalProfile.Animal.BreedId,
                    AnimalTypeId = animalProfile.Animal.AnimalTypeId
                };

                filter.NearbyS2CellIds.Add(animalProfile.S2CellId); // profile s2 cell is included in search

                var animalProfiles = await _profileRepository
                    .GetAnimalProfilesAsync(
                        AnimalProfileFilterBuilder.BuildAnimalProfileFilter(filter), 
                        cancellationToken);

                var animalDetailsDtos = animalProfiles.Select(a =>
                {
                    return new AnimalDetailsDto()
                    {
                        Profile = new AnimalProfileDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Age = a.Age,
                            SexId = a.SexId
                        },
                        Images = a.Images.Select(i => new AnimalImageDto
                        {
                            ImageData = i.ImageData,
                            ImageFormat = i.ImageFormat
                        }).ToList()
                    };
                }).ToImmutableList();

                return Result.Success<ImmutableList<AnimalDetailsDto>>(animalDetailsDtos);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<ImmutableList<AnimalDetailsDto>>(new Error("400", "Operation canceled"));
            }
        }
    }
}
